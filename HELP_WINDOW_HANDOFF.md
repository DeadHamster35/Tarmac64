# Handoff: Tarmac64 Designer Help Window

Owner: Tyler. Status: **design agreed, no code written yet. Phase 1 is NOT yet approved.**

## 0. Read first: working rules (non-negotiable)

- **Approval required.** Workspace rule `require-approval.mdc`: do not create, modify, or delete any file without Tyler's explicit approval. Present each phase's scope or diff and wait for "approved" before editing.
- Stack: **.NET Framework 4.8 WinForms**. No WPF, no .NET upgrade, no new NuGet packages, no WebView2.
- **No HTML/XML content.** Tyler rejected an HTML-in-`WebBrowser` + `HelpTopics.xml` design as at odds with the static-WinForms style of the program. Do not reintroduce it, and do not use CHM or `Help.ShowHelp`.
- Do not touch `Tarmac64 Library` (it is shared with the Library/OverKart pipeline), the course data contract, or build settings.
- No drive-by refactors or cleanup of existing code, including unused-looking windows.
- Match the existing code style: namespace `Tarmac64_Retail`, `public partial class X : Form`, Designer-generated layout, PascalCase control names.

## 1. Goal

Add a traditional help-viewer window to **Tarmac64 Designer** (`Tarmac64 Retail` project). It should guide users through each Course Editor tab and each Tools window, giving every field's **usage** and **expected format**.

## 2. Agreed design

| Decision | Choice |
|---|---|
| Content storage | **C# data** in `Windows/HelpContent.cs` (static list of `HelpTopic` objects). No external files. |
| Layout | **One fixed Designer-built form** (`HelpWindow`), not one UserControl per topic. |
| Field table | **`ListView`** in Details view, with columns Field / Format / Usage. |
| Context-sensitive F1 | Yes, **built alongside the content**: every field gets its F1 mapping in the same phase its text is written. Uses the built-in `Control.HelpRequested` event. See section 5. |
| Windows that no menu opens | **Leave them out**: `CourseUpgrader`, `ModelCompiler` (both files), `HitboxCompiler`, `TextureCompiler`, old `GameBuilder`. |

### Layout

```
+------------------------------------------------------------------+
| [Back] [Forward]                                                 |
+-----------------+------------------------------------------------+
| TreeView        | Course Settings > Fog Settings   (title Label) |
| (Contents)      | Summary text                   (summary Label) |
|                 | +--------------+-------------+---------------+ |
|                 | | Field        | Format      | Usage         | |  ListView
|                 | +--------------+-------------+---------------+ |
|                 | | Enable Fog   | Checkbox    | Turns fog...  | |
| [Search box]    | +--------------+-------------+---------------+ |
|                 | [Selected field full description: read-only    |
|                 |  multiline TextBox]                            |
|                 | See also: <LinkLabels in a FlowLayoutPanel>    |
+-----------------+------------------------------------------------+
```

- The search `TextBox` filters the tree by topic title, field name, or usage text.
- Back/Forward keep a simple history of topic IDs.
- **Single instance:** `HelpWindow.ShowTopic(string topicId, string controlName = null)` creates the window if needed, otherwise brings it to the front. It selects the topic in the tree and, if `controlName` is in a field's `ControlNames`, selects and scrolls to that row.
- **Lookup by control:** `HelpWindow.ShowForControl(string rootTopicId, Control focused)` searches the fields of `rootTopicId` **and its descendant topics** for `focused.Name`. If it finds a match, it opens that topic with the row selected; otherwise it opens `rootTopicId`. Because of this, F1 works on a field as soon as its `HelpField` lists the control name, with no per-field routing code.
  - The search **must be limited to that subtree**, because control names are only unique within one form or user control. For example, `FlagBox` exists in both `ObjectEditor` and `ObjectTypeCompiler`, and `label`/`button1`-style names repeat everywhere.
  - Map only controls that can take focus: TextBox, ComboBox, CheckBox, RadioButton, Button, ListBox/ListView, TreeView, NumericUpDown. Labels and GroupBoxes never receive focus, so mapping them does nothing.

### Data model (`Windows/HelpTopic.cs`)

```csharp
public class HelpTopic
{
    public string Id, Title, ParentId, Summary;
    public List<HelpField> Fields = new List<HelpField>();
    public List<string> SeeAlso = new List<string>();
    public HelpTopic(string id, string title, string parentId) { ... }
}

public class HelpField
{
    public string Name, Format, Usage;
    public string[] ControlNames; // Name of every input control this row covers, used for F1
    public HelpField(string name, string format, string usage, params string[] controlNames) { ... }
}
```

Example entry in `HelpContent.cs`:

```csharp
new HelpTopic("course.settings.fog", "Fog Settings", "course.settings")
{
    Summary = "Distance fog applied to the course geometry.",
    Fields =
    {
        new HelpField("Enable Fog", "Checkbox", "Turns fog on or off.", "FogToggleBox"),
        new HelpField("Color R/G/B/A", "Integer 0-255", "Fog color and alpha.",
            "FogRBox", "FogGBox", "FogBBox", "FogABox"),
    },
    SeeAlso = { "course.settings.sky" },
}
```

### Rules for writing content

- Derive **Format** from code, not guesswork. Check how the handler parses the value (`Convert.ToInt32`, `int.Parse`, `float.Parse`), any clamps, the `OpenFileDialog` filters, and where the value is written in `Tarmac64 Library/TM64_Course.cs` / `TM64_Paths.cs` (byte size and signedness).
- Where the code doesn't show the *purpose* of a field, write `TODO: Tyler` rather than inventing one. Likely cases: Path FX echo, Object Class/Mode/Flag, Min. Y Type, Sky Type, Geometry Modes.
- Game Builder's **Course Header Table** default `BE9178` is the shared `ok_HeaderROM` ROM offset. Document it as "do not change unless your OverKart build moves `ok_HeaderROM`". The help describes the data contract and never changes it.
- `CourseSettings.Designer.cs` appears to contain **duplicate groups**: two "Fog Settings" (`groupBox4`, `groupBox10`) and a "Course Paths" group (`groupBox11`) that overlaps Path Settings. Check `Visible` and parent wiring, and document only what the user actually sees. Ask Tyler if unclear.

## 3. Codebase facts

- Solution: `Tarmac64.sln`. Project: `Tarmac64 Retail/Tarmac64 Designer.csproj` (**old-style csproj; every new file must be added explicitly**):
  ```xml
  <Compile Include="Windows\HelpWindow.cs"><SubType>Form</SubType></Compile>
  <Compile Include="Windows\HelpWindow.Designer.cs"><DependentUpon>HelpWindow.cs</DependentUpon></Compile>
  <Compile Include="Windows\HelpTopic.cs" />
  <Compile Include="Windows\HelpContent.cs" />
  <EmbeddedResource Include="Windows\HelpWindow.resx"><DependentUpon>HelpWindow.cs</DependentUpon></EmbeddedResource>
  ```
- Namespace: `Tarmac64_Retail`. The main form is `Main` (`Main.cs` / `Main.Designer.cs`), with window title "Tarmac 64".
- Windows are opened modelessly, for example `new SongExtractor().Show();` in `Main.cs`.

### Main menu (`Main.Designer.cs`, `menuStrip1`)

| Menu | Items (control name → handler in `Main.cs`) |
|---|---|
| File | New (`newToolStripMenuItem`), Open (`openProjectToolStripMenuItem`, `LoadXML`), Save (`saveProjectToolStripMenuItem`, `SaveXML`), Compile (`exportToolStripMenuItem`, `CompileModel`) |
| Edit | Replace Model, Replace Paths, Import SVL3, Export SVL3 |
| Tools | Ghost Extractor (`oBJToolStripMenuItem` → `GhostExtractor`), Song Extractor (`songExtractorToolStripMenuItem` → `SongExtractor`), Object Compiler (`objectTypeCompilerToolStripMenuItem` → `ObjectTypeCompiler`), Game Buider [sic] (`rOMBuiderToolStripMenuItem` → `GameBuilderPro`), Patch → Create / Apply |
| **Help** (`aboutToolStripMenuItem`, Text = "Help", around line 1037) | Settings (`aboutToolStripMenuItem1` → `SettingsWindow`), About (`aboutToolStripMenuItem2` → `TarmacAbout`) |

`aboutToolStripMenuItem_Click` (Main.cs ~line 1946) is an existing **empty** handler on the Help menu. Leave it alone; the new item gets its own handler.

### Course Editor tabs (`tabControl1` in `Main.Designer.cs`)

| TabPage name | Tab text | Content |
|---|---|---|
| `SurfaceMap` | Surface Mesh | inline: Section, Material, Property, # Verts, # Faces, mode/CC checkboxes (GP, TT, 50, 100, 150, Extra, VS, Battle) |
| `SectionViews` | Section Lists | inline: Section Stats (Objects, Faces), Copy From |
| `TextureData` | Textures | `TextureControl` (`Controls/TextureEditor`) |
| `Settings` | Course Settings | `SettingsControl` (`Controls/CourseSettings`) |
| `PathTab` | Path Settings | `PathControl` (`Controls/PathSettings`) |
| `Object` | Course Objects | `ObjectControl` (`Controls/ObjectEditor`) |
| `tabPage1` | Render Mesh | inline, mirrors Surface Mesh (controls suffixed `R`) |

The 3D viewer is `GLControl` (`Controls/GLViewer`). Its "?" button only toggles `SettingsPanel`; it is not help.

**Field labels per control** (read the `*.Designer.cs` files for exact control names):

- **TextureEditor:** Texture, Texel (+), Codec, Bit Size, Width, Height, Mode S/T, Scroll S/T, Filter, CombineA/B, RenderA/B, Geometry Modes, AlphaMask, Screen, Import/Export (OK64-Texture). Color Combine group: Cycles, Prim Color, Env Color, P. Alpha, E. Alpha, "..." buttons that open `ColorCombineEditor` (Color/Alpha formula editor, Confirm/Cancel).
- **CourseSettings:** Course Name, Credit Text, .ok64.GHOST, Banner Image, Preview Image. Music: .ok64.SONG, Music, T.T. Tempo. Sky - Weather: Enable Sky, Sky Top/Mid/Bot RGB, Type. Min. Y, Min. Y Type. Radar Map: Map Image, Map Coord X/Y, Map Scale, Color RGB, Start Coord X/Y, Line Coord X/Y. Fog: Enable Fog, Color RGBA, Start, End. GameType.
- **PathSettings:** Laps | Finish Line (Sprint A->B / Circuit A->A, GP Laps, Draw Goal Banner). Course Paths (New / Delete / Normalize Path (20), Loaded Path, Path Count, Force Path Distribution). Path FX (Start, End, Power, Base RGB, Adj RGB, +/-). VS Bombs (Index, Type, Marker).
- **ObjectEditor:** object list (+/-), Type (+/-), Mode, Class, Player, Flag, Location XYZ, Rotation XYZ, Velocity XYZ, Angle XYZ, Import .POP3.
- **GLViewer:** FPS, Speed, Render Screens, Draw Paths, Wireframe Hidden, Flash Hover, Textured, Screenshot (#).

### Tools windows to document

- **GhostExtractor** ("Boo - Ghost Extractor"): Load Ghost, Character, Time, Export Raw, OK64.Ghost.
- **SongExtractor** ("♫"): Bank ID, Sequence ID, Export. Prompts for the source ROM, then saves an MIO0-compressed song (`Convert.ToInt32` on both boxes).
- **ObjectTypeCompiler** ("Object Compiler"): Model (...), Blender FBX File, Name, Behavior, Scale, Level Hitbox, Sound, Sound Type, Sound Range, Render Radius. Options: Z-Sort [Slow], Gravity Enabled, Face Camera. Hitbox list (+/-) with Type, Effect, Status, Flag, Solid Object, Size/Pos/Ang XYZ. Save XML / Load XML.
- **GameBuilderPro** (in **Tarmac64 Library**, namespace `Tarmac64_Library`, title "Tarmac Game Builder"): Set Number, Cup Number, course list (Double Click = add, Middle Click = remove), Course Header Table (`BE9178`), Export Debug, ▼ expand, Save, Load, Build.
- **SettingsWindow** ("Tarmac64 Settings"): Course Models dir, Object Models dir, ROM Directory, Import Scale, Scale Mode, Vertex Alpha - Channel 2, Save.
- **Patch Create / Apply** (`Main.cs`): message-box driven. Create: base file, then modified file, then save patch. Apply: base file, then patch file, then save output.

## 4. Topic tree (IDs are suggestions)

```
start                     Getting Started (Tarmac -> Game Builder -> OverKart workflow; project XML vs compiled .ok64)
  start.settings          Settings window
menus                     Menus
  menus.file / menus.edit / menus.patch
viewer                    3D Viewer
course                    Course Editor
  course.surface          Surface Mesh
  course.sections         Section Lists
  course.render           Render Mesh
  course.textures         Textures
    course.textures.combine   Color Combine Editor
  course.settings         Course Settings
    .info .music .sky .miny .radar .fog
  course.paths            Path Settings
    .laps .paths .fx .bombs
  course.objects          Course Objects
tools                     Tools
  tools.ghost / tools.song / tools.objcompiler / tools.gamebuilder
```

## 5. How F1 works, and how it is built up field by field

Tyler's direction: **F1 is not a separate final phase.** Every field gets its F1 mapping in the same phase as its help text, and each window is hooked up in the phase where its topic is written.

### How it works

- When F1 is pressed, Windows sends `WM_HELP`, and WinForms raises `HelpRequested` on the **focused** control. If no handler marks it handled, the base `OnHelpRequested` passes it to the parent, and so on up to the form. One handler on the form therefore catches F1 from any child, and no `KeyPreview` is needed.
- The form-level handler's `sender` is the form, not the focused control, so the handler has to find the focused control itself. Walk `ActiveControl`, and while the current control is a `ContainerControl` (this includes UserControls such as `CourseSettings`) step into its `ActiveControl`. The innermost control is the focused one. Controls inside a GroupBox or Panel come back directly, because those aren't `ContainerControl`s.
- That control is passed to `HelpWindow.ShowForControl(rootTopicId, focused)` (section 2), followed by `hlpevent.Handled = true`.

### The Help menu item and F1 (open question now resolved)

- **Do not** set `ShortcutKeys = Keys.F1` on the User Guide menu item. A menu shortcut is processed in `ProcessCmdKey` before `WM_HELP`, so it would swallow F1 and context help would never fire.
- Set `ShortcutKeyDisplayString = "F1"` instead, so the menu still shows F1 while `HelpRequested` does the actual work.
- Clicking the menu item opens `start`. Pressing F1 in `Main` opens the current tab's topic, or the focused field within it.

### Wiring per window

| Window | Where the handler goes | Root topic |
|---|---|---|
| `Main` | `this.HelpRequested` in `Main.cs`. The root topic comes from `tabControl1.SelectedTab` (see the map below). | per tab |
| `ColorCombineEditor` | its own `HelpRequested` (Retail) | `course.textures.combine` |
| `SettingsWindow`, `GhostExtractor`, `SongExtractor`, `ObjectTypeCompiler` | each form's own `HelpRequested` handler (Retail) | `start.settings`, `tools.ghost`, `tools.song`, `tools.objcompiler` |
| `GameBuilderPro` | **Do not edit it (Tarmac64 Library).** Attach from `Main.cs` after construction, `GB.HelpRequested += ...`, in **both** `rOMBuiderToolStripMenuItem_Click` and `proToolStripMenuItem_Click`. The handler needs the focused control, so use `((Form)sender).ActiveControl` with the same walk. | `tools.gamebuilder` |

**Tab to root topic map** for `Main`: `SurfaceMap` → `course.surface`, `SectionViews` → `course.sections`, `TextureData` → `course.textures`, `Settings` → `course.settings`, `PathTab` → `course.paths`, `Object` → `course.objects`, `tabPage1` → `course.render`.

**3D viewer:** if the focused control is `GLControl`, or one of its children, use `viewer` as the root instead of the tab's topic.

### Per-field rule (applies to every phase)

When a `HelpField` is written, fill in `ControlNames` with **every** focusable input control that the row covers, for example all of R/G/B/A, or both X and Y. Then test F1 on each of those controls before handing the phase over for review. A field with text but no control mapping is not finished.

## 6. Phases (each needs Tyler's approval before editing, then a diff review)

### Phase 1: viewer, F1 plumbing, Getting Started, Course Settings (scope proposed, awaiting "approved")

- Add `Windows/HelpTopic.cs`, `Windows/HelpContent.cs` (the full topic tree, with placeholder topics except **Getting Started** and **Course Settings**), and `Windows/HelpWindow.cs/.Designer.cs/.resx`, including `ShowTopic` and `ShowForControl`.
- Add the csproj entries from section 3.
- `Main.Designer.cs`: add a **User Guide** item at the top of the Help menu's `DropDownItems`, with `ShortcutKeyDisplayString = "F1"` and no `ShortcutKeys`.
- `Main.cs`: the menu click handler (`ShowTopic("start")`), plus the `HelpRequested` handler with the `ActiveControl` walk, the full tab map, and the `GLControl` check. Placeholder topics simply open at their root, so F1 already works on every tab.
- Course Settings: every field gets its `ControlNames`, and F1 is tested on each one.

### Phase 2: Course Editor content and F1

Write Surface Mesh, Section Lists, Render Mesh, Textures, Path Settings, Course Objects, and the 3D viewer, with `ControlNames` on every field. Add the Color Combine Editor topic and its `HelpRequested` handler.

### Phase 3: Tools and menus content and F1

Write the Settings window, Ghost Extractor, Song Extractor, Object Compiler, Game Builder, Patch, and the File/Edit menus, with `ControlNames` on every field. In the same phase, add the `HelpRequested` handlers for those windows, including the `GameBuilderPro` attachment from `Main.cs`.

## 7. Done when

- The solution builds in Visual Studio (Framework 4.8) with no new warnings in the touched files.
- Help → User Guide opens a single-instance window. The tree, search, Back/Forward, field selection, and see-also links all work.
- Every topic in section 4 has content. Unknown purposes are marked `TODO: Tyler` and listed for him in the final summary.
- **At the end of each phase**, F1 on every field documented in that phase opens the right topic with that field's row selected. F1 on an unmapped control opens the root topic of its tab or window.
