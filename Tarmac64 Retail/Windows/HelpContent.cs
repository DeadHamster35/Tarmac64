using System.Collections.Generic;

namespace Tarmac64_Retail
{
    public static class HelpContent
    {
        public static readonly List<HelpTopic> Topics = new List<HelpTopic>
        {
            new HelpTopic("start", "Getting Started", null)
            {
                Summary = "Tarmac64 Designer is where you build a custom Mario Kart 64 course.\r\n\r\n" +
                    "• File → Open / Save: your project (*.ok64.Save)\r\n" +
                    "• File → Compile: the playable course (*.ok64.Course)\r\n" +
                    "• Tools → Game Builder: put courses into a Mario Kart 64 (USA) ROM\r\n" +
                    "• Help → Settings: folders for courses, objects, and ROMs, plus model import size\r\n\r\n" +
                    "After you compile, Game Builder writes the courses into the ROM. Leave the Course Header Table at BE9178 unless you were told to move it for a special OverKart build.",
                SeeAlso = { "start.settings", "course.settings", "tools.gamebuilder", "menus.file" }
            },
            new HelpTopic("start.settings", "Settings", "start")
            {
                Summary = "Help → Settings remembers the folders Tarmac should start in, and how 3D models are sized on import.\r\n\r\n" +
                    "Click Save after you change anything here.",
                Fields =
                {
                    new HelpField("Course Models", "Folder",
                        "The folder Tarmac opens first when you load, save, or compile a course, and when Game Builder asks you to pick a course.",
                        "CourseDIRBox", "CourseDIRButton"),
                    new HelpField("Object Models", "Folder",
                        "The folder used when you load or save an object type, or pick an object model.",
                        "ObjectDIRBox", "ObjectDIRButton"),
                    new HelpField("ROM Directory", "Folder",
                        "The folder Game Builder starts in when you pick a ROM.\r\n\r\n" +
                        "This box may look empty when you reopen Settings even if a folder was saved.",
                        "ROMDIRBox", "button1"),
                    new HelpField("Import Scale", "Number",
                        "Makes imported 3D models larger or smaller. 1 leaves the size unchanged.\r\n\r\n" +
                        "The same scale is applied to hitboxes loaded with a model.",
                        "ScaleBox"),
                    new HelpField("Vertex Alpha - Channel 2", "On or off",
                        "When on, the second color channel of the course model is used for see-through parts of the mesh.",
                        "AlphaBox"),
                    new HelpField("Scale Mode", "Choice",
                        "Which 3D program the model came from, so Tarmac can size it correctly.\r\n\r\n" +
                        "• Static: no extra scaling\r\n" +
                        "• Blender: treats the model as a Blender export (Blender units are 100 times larger)\r\n" +
                        "• 3DSMax: uses the model's own placement, without extra scaling",
                        "ScaleModeBox"),
                    new HelpField("Save", "Button",
                        "Stores these folders and import options.\r\n\r\n" +
                        "If Import Scale is not a number, Tarmac shows \"Invalid Scale Value\".",
                        "button3"),
                },
                SeeAlso = { "tools.gamebuilder", "tools.objcompiler", "menus.file" }
            },

            new HelpTopic("menus", "Menus", null)
            {
                Summary = "The menus along the top of the main window.\r\n\r\n" +
                    "• File: open and compile the course\r\n" +
                    "• Edit: replace the model or paths, or copy section lists\r\n" +
                    "• Tools: ghosts, songs, objects, Game Builder, and patches\r\n" +
                    "• Help → User Guide (F1): this window\r\n" +
                    "• Help → Settings: working folders and import size",
                SeeAlso = { "menus.file", "menus.edit", "menus.patch", "tools.gamebuilder", "start.settings" }
            },
            new HelpTopic("menus.file", "File", "menus")
            {
                Summary = "Create, open, save, and compile a course.\r\n\r\n" +
                    "Open and Save use a project file (*.ok64.Save).\r\n" +
                    "Compile writes a playable course (*.ok64.Course) that Game Builder can put in a ROM.",
                Fields =
                {
                    new HelpField("New", "Menu",
                        "Starts a new project. You pick a course model, then the path files.",
                        "newToolStripMenuItem"),
                    new HelpField("Open", "Menu",
                        "Opens a saved Tarmac project (*.ok64.Save) from the Course Models folder.",
                        "openProjectToolStripMenuItem"),
                    new HelpField("Save", "Menu",
                        "Saves the current project (*.ok64.Save) to the Course Models folder.\r\n\r\n" +
                        "This includes settings, paths, textures, objects, sections, and the visible mesh.",
                        "saveProjectToolStripMenuItem"),
                    new HelpField("Compile", "Menu",
                        "Builds a playable course file (*.ok64.Course) for Game Builder, in the Course Models folder.",
                        "exportToolStripMenuItem"),
                },
                SeeAlso = { "tools.gamebuilder", "start.settings", "menus.edit" }
            },
            new HelpTopic("menus.edit", "Edit", "menus")
            {
                Summary = "Replace the course model or paths without starting over, or copy which objects each camera section can see.\r\n\r\n" +
                    "Import and Export SVL3 need a project to already be open.",
                Fields =
                {
                    new HelpField("Replace Model", "Menu",
                        "Brings in a new course model and keeps the rest of the project.",
                        "replaceModelToolStripMenuItem"),
                    new HelpField("Replace Paths", "Menu",
                        "Brings in new path files and keeps the rest of the project.",
                        "replacePathsToolStripMenuItem"),
                    new HelpField("Import SVL3", "Menu",
                        "Replaces which objects each camera section can see, from an SVL3 file.\r\n\r\n" +
                        "Does nothing if no project is open.",
                        "importSVL3ToolStripMenuItem"),
                    new HelpField("Export SVL3", "Menu",
                        "Saves the current camera-section object lists to an SVL3 file.\r\n\r\n" +
                        "Does nothing if no project is open.",
                        "exportSVL3ToolStripMenuItem"),
                },
                SeeAlso = { "course.sections", "course.render", "menus.file" }
            },
            new HelpTopic("menus.patch", "Patch", "menus")
            {
                Summary = "Tools → Patch builds or applies a Tarmac patch file (*.ok64.Patch).\r\n\r\n" +
                    "Game Builder also applies Resources/PatchData.ok64.Patch automatically when the ROM has not been patched yet.",
                Fields =
                {
                    new HelpField("Create", "Menu",
                        "Builds a patch from two files.\r\n\r\n" +
                        "1. The original, unmodified file\r\n" +
                        "2. The changed file\r\n" +
                        "3. Save as *.ok64.Patch",
                        "createToolStripMenuItem"),
                    new HelpField("Apply", "Menu",
                        "Applies a patch to an original file.\r\n\r\n" +
                        "1. The original, unmodified file\r\n" +
                        "2. The *.ok64.Patch\r\n" +
                        "3. Save the result (usually a *.z64 ROM)",
                        "applyToolStripMenuItem"),
                },
                SeeAlso = { "tools.gamebuilder" }
            },

            new HelpTopic("viewer", "3D Viewer", null)
            {
                Summary = "Preview the course in 3D on the right side of the window.\r\n\r\n" +
                    "The \"?\" button only shows or hides the viewer options. Press F1 (or Help → User Guide) for this help.",
                Fields =
                {
                    new HelpField("3D View", "View",
                        "The course preview. Click and drag to look around.",
                        "GLWindow"),
                    new HelpField("?", "Button",
                        "Shows or hides the viewer options. This is not the User Guide.",
                        "button4"),
                    new HelpField("Screenshot (#)", "Button",
                        "Saves the current view as a PNG picture.",
                        "ScreenshotBtn"),
                    new HelpField("Speed", "Read-only",
                        "How fast the 3D view camera moves. Change it with the 3D editor's keyboard controls; this box only shows the current value.",
                        "SpeedBox"),
                    new HelpField("FPS", "Number",
                        "How smooth the preview is while the mouse is over it. Higher uses more computer time.\r\n\r\n" +
                        "Use 0 through 144. When the mouse leaves the view, the preview slows down to save work.",
                        "FPSBox"),
                    new HelpField("Textured", "On or off",
                        "When on, meshes draw with their textures. When off, they draw untextured.",
                        "CheckboxTextured"),
                    new HelpField("Draw Paths", "On or off",
                        "When on, the race paths are drawn in the preview.",
                        "CheckboxPaths"),
                    new HelpField("Flash Hover", "On or off",
                        "When on, the mesh or object under the cursor flashes so you can tell what you are pointing at.",
                        "CheckboxHover"),
                    new HelpField("Wireframe Hidden", "On or off",
                        "When on, you can see the outlines of meshes that sit behind other surfaces.",
                        "chkWireframe"),
                    new HelpField("Render Screens", "On or off",
                        "When on, textures assigned to a screen corner (HUD-style pictures) are drawn.\r\n\r\n" +
                        "Turn this off if those pictures get in the way while you edit.",
                        "RenderCheckbox"),
                },
                SeeAlso = { "course.surface", "course.render", "course.paths" }
            },

            new HelpTopic("course", "Course Editor", null)
            {
                Summary = "The tabs on the left edit the loaded course.\r\n\r\n" +
                    "• Surface Mesh: collision (what the karts drive on)\r\n" +
                    "• Section Lists: which objects each camera section can see\r\n" +
                    "• Textures: pictures and how they draw\r\n" +
                    "• Course Settings: name, sky, fog, map, music\r\n" +
                    "• Path Settings: laps, AI paths, and effects along the track\r\n" +
                    "• Course Objects: items placed in the course\r\n" +
                    "• Render Mesh: the visible course model\r\n\r\n" +
                    "Press F1 on a field to open its topic with that row selected.",
                SeeAlso = { "course.surface", "course.sections", "course.render", "course.textures", "course.settings", "course.paths", "course.objects", "viewer" }
            },
            new HelpTopic("course.surface", "Surface Mesh", "course")
            {
                Summary = "Collision meshes are what karts drive on, bounce off, and fall through.\r\n\r\n" +
                    "Pick a mesh, then set which camera section it belongs to, how it feels to drive on, extra behavior, and which game modes and speeds use it.",
                Fields =
                {
                    new HelpField("Mesh list", "List",
                        "All collision meshes in the course. Select one to edit it.",
                        "SurfaceMeshListBox"),
                    new HelpField("Section", "Choice",
                        "Which camera section this collision belongs to.\r\n\r\n" +
                        "The game only uses nearby sections, so putting a mesh in the right section keeps collision accurate without loading the whole course at once.",
                        "surfsectionBox"),
                    new HelpField("Property", "Choice",
                        "Extra behavior when a kart hits this mesh.\r\n\r\n" +
                        "• Standard: normal collision\r\n" +
                        "• Force Tumble: the kart tumbles\r\n" +
                        "• No Collision: the kart passes through\r\n" +
                        "• Darken Player: the kart is drawn darker (tunnels, caves)\r\n" +
                        "• Out of Bounds: treated as off-course",
                        "surfpropertybox"),
                    new HelpField("Material", "Choice",
                        "How the kart feels on this surface: grip, splash, boost, ice slide, and similar.\r\n\r\n" +
                        "Stock types include Solid, Dirt, Cement, Ice, and Grass.\r\n" +
                        "Custom C- types include boosts, lava, out of bounds, and walls.",
                        "surfmaterialBox"),
                    new HelpField("# Verts / # Faces", "Read-only",
                        "How many points and triangles this mesh has. These boxes cannot be edited.",
                        "SurfaceVertBox", "SurfaceFaceBox"),
                    new HelpField("Grand Prix / Time Trial / VS / Battle", "On or off",
                        "When checked, this collision is used in that game mode. Uncheck to hide it from that mode.",
                        "GPBoxC", "TTBoxC", "VSBoxC", "BattleBoxC"),
                    new HelpField("50 / 100 / 150 / Extra", "On or off",
                        "When checked, this collision is used at that speed class. Uncheck to hide it from 50cc, 100cc, 150cc, or Extra.",
                        "FiftyBoxC", "HundredBoxC", "HundredFiftyBoxC", "ExtraBoxC"),
                },
                SeeAlso = { "course.render", "course.sections" }
            },
            new HelpTopic("course.sections", "Section Lists", "course")
            {
                Summary = "Each camera section only draws the objects you check here. That keeps far-away pieces from drawing when the camera cannot see them.",
                Fields =
                {
                    new HelpField("Section", "Choice",
                        "Which camera section's object list you are editing.",
                        "sectionBox"),
                    new HelpField("Copy From", "Button",
                        "Replaces this section's object list with the list from another section. Use this when two camera views should show the same objects.",
                        "CopyBtn", "CopySectionIndexBox"),
                    new HelpField("Faces / Objects", "Read-only",
                        "How many faces and objects are in the selected section. These boxes cannot be edited.",
                        "faceBox", "objectCountBox"),
                    new HelpField("Section tree", "List",
                        "Check an object (or a group) to include it in this camera section. Uncheck to leave it out.\r\n\r\n" +
                        "See-through objects use the same list.",
                        "masterBox"),
                },
                SeeAlso = { "course.render", "course.surface" }
            },
            new HelpTopic("course.render", "Render Mesh", "course")
            {
                Summary = "Visible meshes are what you see in the course. Assign a texture, then choose which game modes and speeds draw each mesh.",
                Fields =
                {
                    new HelpField("Mesh list", "List",
                        "All visible meshes in the course. Select one to edit it.",
                        "RenderMeshListBox"),
                    new HelpField("Section", "Choice",
                        "Shown to match the Surface Mesh tab, but this list is not saved.\r\n\r\n" +
                        "Set camera sections for collision on Surface Mesh, and which objects a section can see on Section Lists.",
                        "comboBox3"),
                    new HelpField("Property", "Choice",
                        "Shown to match the Surface Mesh tab, but this list is not saved.\r\n\r\n" +
                        "Collision properties (tumble, no collision, out of bounds, and so on) are set on Surface Mesh.",
                        "comboBox1"),
                    new HelpField("Material", "Choice",
                        "Which texture this visible mesh uses. The names come from the Textures tab.",
                        "RenderMaterialBox"),
                    new HelpField("# Verts / # Faces", "Read-only",
                        "How many points and triangles this mesh has. These boxes cannot be edited.",
                        "RenderVertBox", "RenderFaceBox"),
                    new HelpField("Grand Prix / Time Trial / VS / Battle", "On or off",
                        "When checked, this mesh is drawn in that game mode. Uncheck to hide it from that mode.",
                        "GPBoxR", "TTBoxR", "VSBoxR", "BattleBoxR"),
                    new HelpField("50 / 100 / 150 / Extra", "On or off",
                        "When checked, this mesh is drawn at that speed class. Uncheck to hide it from 50cc, 100cc, 150cc, or Extra.",
                        "CC50BoxR", "CC100BoxR", "CC150BoxR", "CCExtraBoxR"),
                },
                SeeAlso = { "course.surface", "course.textures", "course.sections" }
            },
            new HelpTopic("course.textures", "Textures", "course")
            {
                Summary = "Pictures used by the course, and how they wrap, scroll, and blend.\r\n\r\n" +
                    "Color mixing and see-through settings are on Color Combine Editor.",
                Fields =
                {
                    new HelpField("Material", "Choice",
                        "Which course texture you are editing.",
                        "MaterialSelect"),
                    new HelpField("OK64-Texture Import / Export", "Button",
                        "Load or save this texture's settings as a Tarmac texture file (*.ok64.Texture), so you can reuse it on another course.",
                        "button4", "button3"),
                    new HelpField("Texel", "Choice",
                        "Which picture layer you are editing. Texel 0 is the main picture.\r\n\r\n" +
                        "+ adds or removes a second picture (Texel 1) for two-layer materials.",
                        "TexelSelect", "TexelAddBtn"),
                    new HelpField("Texture", "File",
                        "The picture file for this layer. Click ... to browse.",
                        "TextureBox", "TexelReplaceBTN"),
                    new HelpField("AlphaMask", "On or off",
                        "When on, pick a second picture that controls which parts are see-through. When off, that mask is cleared.",
                        "AlphaMaskCheckbox", "alphaMaskBox"),
                    new HelpField("Codec", "Choice",
                        "How the picture is stored.\r\n\r\n" +
                        "• RGBA: full color\r\n" +
                        "• CI: a small color palette (uses less space)\r\n" +
                        "• IA: gray with see-through\r\n" +
                        "• I: gray only",
                        "CodecBox"),
                    new HelpField("Bit Size", "Choice",
                        "How much color detail each pixel has. Higher looks better and uses more space.\r\n\r\n" +
                        "Typical choices are 4, 8, 16, or 32.",
                        "BitBox"),
                    new HelpField("Width / Height", "Read-only",
                        "The picture's size in pixels. These come from the image and cannot be typed.",
                        "widthBox", "heightBox"),
                    new HelpField("Mode S / Mode T", "Choice",
                        "What happens at the edges of the picture, left-right (S) and up-down (T).\r\n\r\n" +
                        "• No-Wrap: stop at the edge\r\n" +
                        "• Wrap: repeat the picture\r\n" +
                        "• Mirror: repeat it flipped\r\n" +
                        "• Clamp: hold the edge color\r\n" +
                        "• Mirrored Clamp: a mix of mirror and clamp",
                        "SFlagBox", "TFlagBox"),
                    new HelpField("Scroll S / Scroll T", "Number",
                        "How fast the picture slides sideways (S) and up or down (T). Use this for water, belts, and similar moving surfaces. 0 does not scroll.",
                        "textureScrollSBox", "textureScrollTBox"),
                },
                SeeAlso = { "course.textures.combine", "course.render" }
            },
            new HelpTopic("course.textures.combine", "Color Combine Editor", "course.textures")
            {
                Summary = "How a texture mixes with lighting and extra colors, and whether it draws solid or see-through.\r\n\r\n" +
                    "Most courses can leave the presets. The ... buttons open a formula window if you need a custom mix.",
                Fields =
                {
                    new HelpField("Cycles", "Choice",
                        "How many mixing passes to run.\r\n\r\n" +
                        "• One-Cycle: simpler and faster\r\n" +
                        "• Two-Cycle: can blend more layers (uses Combine B as well)",
                        "CycleBox"),
                    new HelpField("CombineA", "Choice",
                        "How the first pass mixes the texture, lighting, and extra colors.\r\n\r\n" +
                        "Click ... to edit the mix by hand.",
                        "CombineBoxA", "AdvanceABtn"),
                    new HelpField("CombineB", "Choice",
                        "How the second pass mixes colors. Used when Cycles is Two-Cycle.\r\n\r\n" +
                        "Click ... to edit the mix by hand.",
                        "CombineBoxB", "AdvanceBBtn"),
                    new HelpField("Color formula A/B/C/D", "Choice",
                        "A custom color mix of the form (A − B) × C + D.\r\n\r\n" +
                        "Confirm applies it. Cancel closes without changing the material.",
                        "ColorA", "ColorB", "ColorC", "ColorD"),
                    new HelpField("Alpha formula A/B/C/D", "Choice",
                        "A custom see-through mix of the form (A − B) × C + D.",
                        "AlphaA", "AlphaB", "AlphaC", "AlphaD"),
                    new HelpField("Confirm / Cancel", "Button",
                        "Confirm applies the formula to the material and closes. Cancel closes without applying.",
                        "button1", "button2"),
                    new HelpField("RenderA / RenderB", "Choice",
                        "How the material is drawn — solid, see-through, and similar — for the first and second pass.",
                        "RenderBoxA", "RenderBoxB"),
                    new HelpField("Filter", "Choice",
                        "How pixels are smoothed when the camera is close or far.\r\n\r\n" +
                        "• Point: sharp pixels\r\n" +
                        "• Bilerp: smoother\r\n" +
                        "• Average: a softer mix",
                        "FilterBox"),
                    new HelpField("Prim Color / Env Color", "Color",
                        "Two extra colors the mixer can use. Click the button to pick a color.",
                        "PrimColorBTN", "EnvColorBTN"),
                    new HelpField("P. Alpha / E. Alpha", "Number",
                        "How see-through those extra colors are. 0 is fully clear, 255 is fully solid.",
                        "PAlphaBox", "EAlphaBox"),
                    new HelpField("Geometry Modes", "List",
                        "Extra drawing options. The defaults are fine for most materials.",
                        "GeoModeBox"),
                    new HelpField("Screen", "Choice",
                        "Puts this picture on a corner of the screen (a HUD-style overlay) instead of on the course.\r\n\r\n" +
                        "None is a normal course texture.",
                        "screenBox"),
                },
                SeeAlso = { "course.textures" }
            },

            new HelpTopic("course.settings", "Course Settings", "course")
            {
                Summary = "Course-wide options: name and pictures, music, sky, water height, radar map, and fog.\r\n\r\n" +
                    "Each group has its own topic. Press F1 on a field to open that group with the row selected.",
                Fields =
                {
                    new HelpField("GameType", "Choice",
                        "What kind of course this is.\r\n\r\n" +
                        "• Race: Grand Prix, Time Trial, and VS\r\n" +
                        "• Battle: battle arenas\r\n\r\n" +
                        "In Game Builder, the Battle cup only accepts battle courses, and race cups reject battle courses.",
                        "GameTypeBox"),
                },
                SeeAlso = { "course.settings.info", "course.settings.music", "course.settings.sky", "course.settings.miny", "course.settings.radar", "course.settings.fog" }
            },
            new HelpTopic("course.settings.info", "Course Info", "course.settings")
            {
                Summary = "The name, credits, and pictures shown when picking this course, plus an optional staff ghost.",
                Fields =
                {
                    new HelpField("Course Name", "Text",
                        "The name shown in Game Builder and on the compiled course.",
                        "CourseNameBox"),
                    new HelpField("Credit Text", "Text",
                        "Credits shown with the course.",
                        "CreditsBox"),
                    new HelpField("Preview Image", "Picture",
                        "The picture shown when choosing this course. Click ... to browse.\r\n\r\n" +
                        "Use a normal picture file (PNG, BMP, and similar). Size is not checked here.",
                        "previewBox", "previewBtn"),
                    new HelpField("Banner Image", "Picture",
                        "The banner shown with the course. Click ... to browse.\r\n\r\n" +
                        "Use a normal picture file. Size is not checked here.",
                        "bannerBox", "bannerBtn"),
                    new HelpField(".ok64.GHOST", "File",
                        "An optional staff ghost for Time Trial. Click ... to browse. Leave this empty if you do not have a ghost.",
                        "ghostBox", "ghostBtn"),
                },
                SeeAlso = { "course.settings" }
            },
            new HelpTopic("course.settings.music", "Music", "course.settings")
            {
                Summary = "The course music: a stock Mario Kart 64 track, or a custom song file, plus Time Trial speed.",
                Fields =
                {
                    new HelpField("Music", "Choice",
                        "Which stock track to play (Raceways, Rainbow Road, and so on).\r\n\r\n" +
                        "If you set a custom .ok64.SONG file, that file is used instead of this list.",
                        "songBox"),
                    new HelpField(".ok64.SONG", "File",
                        "A custom song extracted with Song Extractor. When this path is set, it replaces the stock Music choice.",
                        "oksongBox", "songBtn"),
                    new HelpField("T.T. Tempo", "Number",
                        "How many gameplay steps run for each drawn frame in Time Trial. Default is 2.\r\n\r\n" +
                        "Raise this on a heavy course so Time Trial keeps a steady speed. Grand Prix and VS already adjust themselves; changing this would break ghost saves, so it is Time Trial only.",
                        "TempoTTBox"),
                },
                SeeAlso = { "course.settings", "tools.song" }
            },
            new HelpTopic("course.settings.sky", "Sky - Weather", "course.settings")
            {
                Summary = "The sky behind the course: on or off, gradient colors, and clouds, stars, or weather.",
                Fields =
                {
                    new HelpField("Enable Sky", "On or off",
                        "Turns the sky on or off. Turn it off for indoor courses or if you draw your own sky in the mesh.",
                        "SkyBoxCheckBox"),
                    new HelpField("Sky Top R/G/B", "Color",
                        "Color at the top of the sky. Use the boxes or the color button.",
                        "SkyRT", "SkyGT", "SkyBT", "ColorPickT"),
                    new HelpField("Sky Mid R/G/B", "Color",
                        "Color in the middle of the sky. Use the boxes or the color button.",
                        "SkyRM", "SkyGM", "SkyBM", "ColorPickMT"),
                    new HelpField("Sky Bot R/G/B", "Color",
                        "Color at the horizon. Use the boxes or the color button.",
                        "SkyRB", "SkyGB", "SkyBB", "ColorPickB"),
                    new HelpField("Sky Type", "Choice",
                        "What appears in the sky.\r\n\r\n" +
                        "• Clear: empty sky\r\n" +
                        "• Clouds: Mario Raceway-style clouds\r\n" +
                        "• Stars: a night sky\r\n" +
                        "• Weather: snow or rain",
                        "skyBox"),
                    new HelpField("Type", "Choice",
                        "Which cloud set or weather to use. The list changes with Sky Type.\r\n\r\n" +
                        "Clouds: Mario Raceway, Koopa Beach, Moo Moo, Luigi Raceway, Royal Raceway, Sherbet.\r\n" +
                        "Weather: Snow or Rain.\r\n" +
                        "Clear and Stars: not used.",
                        "weatherBox"),
                },
                SeeAlso = { "course.settings.fog", "course.settings" }
            },
            new HelpTopic("course.settings.miny", "Min. Y", "course.settings")
            {
                Summary = "The height where a kart hitting the bottom is treated as water, lava, ice, fire, or falling into the void.",
                Fields =
                {
                    new HelpField("Min. Y", "Number",
                        "The height of that bottom plane. Karts that reach this height splash, burn, or fall, depending on Min. Y Type.",
                        "waterBox"),
                    new HelpField("Min. Y Type", "Choice",
                        "What happens when a kart hits that height.\r\n\r\n" +
                        "• Water: a normal splash; Lakitu can recover you\r\n" +
                        "• Void: falling off the course\r\n" +
                        "• Lava: a lava splash and recovery\r\n" +
                        "• Ice: an ice splash and recovery\r\n" +
                        "• Fire: a Super Mario 64-style fire surface, similar to lava",
                        "WaterTypeBox"),
                },
                SeeAlso = { "course.settings" }
            },
            new HelpTopic("course.settings.radar", "Radar Map", "course.settings")
            {
                Summary = "The small course map shown during a race: the picture, where it sits on screen, and how karts are placed on it.",
                Fields =
                {
                    new HelpField("Map Image", "Picture",
                        "The minimap picture. Click ... to browse. A simple high-contrast drawing of the track works best.",
                        "mapBox", "mapBtn"),
                    new HelpField("Map Coord X/Y", "Number",
                        "Where the map sits on the screen. Higher X moves it right; higher Y moves it down. Common starting values are 260, 170.",
                        "mapXBox", "mapYBox"),
                    new HelpField("Start Coord X/Y", "Number",
                        "Where the start/finish is marked on the map. Common starting values are 6, 28.",
                        "startXBox", "startYBox"),
                    new HelpField("Line Coord X/Y", "Number",
                        "Where the starting-line graphic is drawn on the map.",
                        "lineXBox", "lineYBox"),
                    new HelpField("Map Scale", "Number",
                        "How large the track is drawn on the map. Larger numbers zoom out. 1.55 is a typical starting value.",
                        "MapScaleBox"),
                    new HelpField("Color R/G/B", "Color",
                        "The tint of the minimap. Use the boxes or the color button.",
                        "MapRBox", "MapGBox", "MapBBox", "ColorPickMap"),
                },
                SeeAlso = { "course.settings" }
            },
            new HelpTopic("course.settings.fog", "Fog Settings", "course.settings")
            {
                Summary = "Distance fog that fades the course into a color as it gets farther from the camera.",
                Fields =
                {
                    new HelpField("Enable Fog", "On or off",
                        "Turns fog on or off. Distances are only used when fog is on.",
                        "FogToggleBox"),
                    new HelpField("Start", "Number",
                        "How close the camera is when fog begins. Higher means the course stays clear farther away. 900 is a typical starting value.",
                        "FogStartBox"),
                    new HelpField("End", "Number",
                        "How far away the fog is fully thick. Keep this at 1000 or below. 1000 is a typical starting value.",
                        "FogEndBox"),
                    new HelpField("Color R/G/B/A", "Color",
                        "The fog color. The color button sets red, green, and blue; A is how solid the fog is (255 is fully solid).",
                        "FogRBox", "FogGBox", "FogBBox", "FogABox", "FogCButton"),
                },
                SeeAlso = { "course.settings.sky", "course.settings" }
            },

            new HelpTopic("course.paths", "Path Settings", "course")
            {
                Summary = "Laps, computer-racer paths, effects along the track, and bomb karts for VS.\r\n\r\n" +
                    "Press F1 on a field to open its group.",
                SeeAlso = { "course.paths.laps", "course.paths.paths", "course.paths.fx", "course.paths.bombs" }
            },
            new HelpTopic("course.paths.laps", "Laps | Finish Line", "course.paths")
            {
                Summary = "Whether the course is a loop or a point-to-point race, and how many Grand Prix laps to run.",
                Fields =
                {
                    new HelpField("Circuit Race (A->A)", "Choice",
                        "A closed loop. Racers cross the start/finish each lap. Grand Prix laps can be set (default 3).",
                        "CircuitRadio"),
                    new HelpField("Sprint Race (A->B)", "Choice",
                        "A point-to-point race from start to finish. Grand Prix laps is locked to 1.",
                        "SprintRadio"),
                    new HelpField("GP Laps", "Number",
                        "How many laps Grand Prix uses on a circuit. Use 1 through 9. Sprint races ignore this.",
                        "LapCountBox"),
                    new HelpField("Draw Goal Banner", "On or off",
                        "When on, the finish-line banner is drawn. Turn it off if your course already has its own banner, or you do not want one.",
                        "GoalBannerBox"),
                },
                SeeAlso = { "course.paths.paths", "course.settings.radar" }
            },
            new HelpTopic("course.paths.paths", "Course Paths", "course.paths")
            {
                Summary = "The routes computer racers follow. You can have several paths so they do not all take the same line.",
                Fields =
                {
                    new HelpField("Force Path Distribution", "On or off",
                        "When on, computer racers are pushed onto different paths instead of piling onto the same one. Use this when you have more than one path.",
                        "DistributeBox"),
                    new HelpField("Path Count", "Number",
                        "How many paths the course uses. Mario Kart 64 supports up to 4. This should match the number of paths you loaded.",
                        "PathCountBox"),
                    new HelpField("Loaded Path", "Choice",
                        "Which path New, Delete, and Normalize act on.",
                        "LoadedPathBox"),
                    new HelpField("New Path / Delete Path", "Button",
                        "Add a path, or remove the selected one.",
                        "NewPathButton", "DeletePathButton"),
                    new HelpField("Normalize Path (20)", "Button",
                        "Rebuilds the selected path so markers are 20 units apart. Even spacing helps computer racers follow the line smoothly.\r\n\r\n" +
                        "You will be asked whether the path is a loop.",
                        "NormalizePathButton"),
                },
                SeeAlso = { "course.paths.laps", "course.paths.fx", "viewer" }
            },
            new HelpTopic("course.paths.fx", "Path FX", "course.paths")
            {
                Summary = "Effects that apply between two path markers — echo, color, camera height, air control, and similar.\r\n\r\n" +
                    "Use + and − to add or remove an effect. Start and End are marker numbers along the path.",
                Fields =
                {
                    new HelpField("Type", "Choice",
                        "What this effect does between Start and End.\r\n\r\n" +
                        "• Echo: change how the engine sounds in that stretch (tunnels, caves)\r\n" +
                        "• Color: fade the kart's color using Base and Adj\r\n" +
                        "• Camera: raise or lower the camera; Power is the height change\r\n" +
                        "• AirControl: let the player steer while airborne over this stretch\r\n" +
                        "• Long Jump: a Lakitu return point after a large gap\r\n" +
                        "• AI Area: keep computer racers from using a simplified shortcut through this stretch\r\n" +
                        "• 3D Weather: turn on 3D weather for player 1 in this stretch",
                        "PathTypeBox"),
                    new HelpField("Index", "Choice",
                        "Which effect you are editing. + adds one. − removes the selected one.",
                        "PathIndexBox", "button1", "button3"),
                    new HelpField("Start / End", "Number",
                        "The first and last path markers this effect covers. The effect is active while a racer is between those markers.",
                        "EchoStartBox", "EchoStopBox"),
                    new HelpField("Power", "Number",
                        "How strong the effect is. For Camera, this is how much the camera height changes. For Echo, it is how strong the echo is.",
                        "EchoPowerBox"),
                    new HelpField("Base R/G/B", "Color",
                        "The starting color for a Color effect. Use the boxes or the swatch.",
                        "BaseR", "BaseG", "BaseB", "ColorPickBase"),
                    new HelpField("Adj R/G/B", "Color",
                        "The color the kart fades toward for a Color effect. Use the boxes or the swatch.",
                        "AdjR", "AdjG", "AdjB", "ColorPickAdjust"),
                },
                SeeAlso = { "course.paths.paths" }
            },
            new HelpTopic("course.paths.bombs", "VS Bombs", "course.paths")
            {
                Summary = "Bomb karts that sit on the course in VS mode.",
                Fields =
                {
                    new HelpField("Index", "Choice",
                        "Which bomb kart you are editing.",
                        "BombIndexBox"),
                    new HelpField("Type", "Choice",
                        "How the bomb kart behaves.\r\n\r\n" +
                        "• Null: unused slot\r\n" +
                        "• Rotate L / Rotate R: turns in place\r\n" +
                        "• Stop: stays still",
                        "BombTypeBox"),
                    new HelpField("Marker", "Number",
                        "Which path marker the bomb kart sits on.",
                        "BombPointBox"),
                },
                SeeAlso = { "course.paths.paths" }
            },
            new HelpTopic("course.objects", "Course Objects", "course")
            {
                Summary = "Items placed in the course: item boxes, trees, custom objects, and similar.\r\n\r\n" +
                    "The list + / − add or remove placed copies. Type + / − add or remove object types.\r\n\r\n" +
                    "Positions and angles use Z-up in the editor (X/Y on the ground, Z is height) and are converted when you compile.",
                Fields =
                {
                    new HelpField("Object list", "List",
                        "Every placed copy in the course.\r\n\r\n" +
                        "+ adds a copy of the current type (or of the selected object's type).\r\n" +
                        "− removes the selected copy. The first entry cannot be removed.",
                        "ObjectListBox", "AddObjBtn", "KillObjBtn"),
                    new HelpField("Import .POP3", "Button",
                        "Places copies from a POP3 path file, using the type that is currently selected.",
                        "ImpPOPButton"),
                    new HelpField("Type", "Choice",
                        "Which object type this copy is. + adds a type. − removes a type.\r\n\r\n" +
                        "Custom types come from Object Compiler.",
                        "ObjectTypeIndexBox", "AddTypeBtn", "KillTypeBtn"),
                    new HelpField("Flag", "Number",
                        "A spare number on this copy. Some behaviors use it — for example Strafe reverses direction when this is not 0. Leave 0 unless a behavior needs it.",
                        "FlagBox"),
                    new HelpField("Mode", "Choice",
                        "Which extra game mode this copy belongs to: Battle, Capture the Flag, or Soccer.\r\n\r\n" +
                        "Only used for the special type used by those modes (type 5).",
                        "ModeBox"),
                    new HelpField("Player", "Number",
                        "Which player this copy is tied to in those extra modes. Only used for type 5.",
                        "BPlayerBox"),
                    new HelpField("Class", "Choice",
                        "What this copy is in those extra modes: a spawn point, a flag, or a base. Only used for type 5.",
                        "ClassBox"),
                    new HelpField("Location X/Y/Z", "Number",
                        "Where this copy sits in the course. The editor is Z-up: X and Y are the ground plane, Z is height.\r\n\r\n" +
                        "These values are converted when the course is compiled.",
                        "LocationXBox", "LocationYBox", "LocationZBox"),
                    new HelpField("Angle X/Y/Z", "Number",
                        "Which way this copy faces, in degrees. Z is rotation around up.\r\n\r\n" +
                        "The checkbox next to Z, when checked, gives newly created copies a random facing (0 through 359).",
                        "AngleXBox", "AngleYBox", "AngleZBox", "RandomZBox"),
                    new HelpField("Velocity X/Y/Z", "Number",
                        "Starting movement in editor space (Z-up). Most objects can leave this at 0. Use it for things that should already be moving when the race starts.",
                        "VelocityXBox", "VelocityYBox", "VelocityZBox"),
                    new HelpField("Rotation X/Y/Z", "Number",
                        "How fast this copy spins in editor space (Z-up). 0 does not spin.",
                        "RotationXBox", "RotationYBox", "RotationZBox"),
                },
                SeeAlso = { "tools.objcompiler", "course.paths" }
            },

            new HelpTopic("tools", "Tools", null)
            {
                Summary = "Extra windows for ghosts, songs, object types, and building a ROM.\r\n\r\n" +
                    "• Ghost Extractor / Song Extractor: pull data from stock files\r\n" +
                    "• Object Compiler: make object types for the Course Objects tab\r\n" +
                    "• Game Builder: put compiled courses into a Mario Kart 64 (USA) ROM",
                SeeAlso = { "tools.ghost", "tools.song", "tools.objcompiler", "tools.gamebuilder", "menus.patch" }
            },
            new HelpTopic("tools.ghost", "Ghost Extractor", "tools")
            {
                Summary = "Tools → Ghost Extractor reads a Controller Pak / memory-card dump that contains Mario Kart 64 ghost data.\r\n\r\n" +
                    "A stock card has two ghost slots. Export writes an OK64.Ghost you can attach on Course Settings.",
                Fields =
                {
                    new HelpField("Load Ghost", "Button",
                        "Opens a memory-card dump and fills Ghost 0 and Ghost 1 when a race time is present. Empty slots show null.",
                        "button1"),
                    new HelpField("Ghost slot", "Choice",
                        "Which of the two card ghosts to view or export. This is only available if at least one slot has data.",
                        "ui_ghostSelect"),
                    new HelpField("Time", "Read-only",
                        "The selected ghost's race time.",
                        "ui_ghostTimeBox"),
                    new HelpField("Character", "Read-only",
                        "Who drove the selected ghost: Mario, Luigi, Yoshi, Toad, D.K., Wario, Peach, or Bowser.",
                        "ui_ghostCharacterSelect"),
                    new HelpField("OK64.Ghost", "Button",
                        "Saves the selected ghost as an OK64.Ghost you can set on Course Settings. Dirty card data is cleaned as it is saved.",
                        "button7"),
                    new HelpField("Export Raw", "Button",
                        "This button is not hooked up yet and does nothing.",
                        "button6"),
                },
                SeeAlso = { "course.settings" }
            },
            new HelpTopic("tools.song", "Song Extractor", "tools")
            {
                Summary = "Tools → Song Extractor copies a track from a Mario Kart 64 ROM so you can use it as a custom course song.",
                Fields =
                {
                    new HelpField("Export", "Button",
                        "Copies the track at Bank ID / Sequence ID from the ROM you pick, then asks where to save the song file.",
                        "button1"),
                    new HelpField("Bank ID", "Number",
                        "Which sound bank to read from. Default is 1.",
                        "bankBox"),
                    new HelpField("Sequence ID", "Number",
                        "Which track in that bank to copy. Default is 1.",
                        "seqBox"),
                },
                SeeAlso = { "course.settings" }
            },
            new HelpTopic("tools.objcompiler", "Object Compiler", "tools")
            {
                Summary = "Tools → Object Compiler makes an object type (*.ok64.OBJECT) you can place on the Course Objects tab.\r\n\r\n" +
                    "Load a model, set how it behaves, sounds, and collides, then save it into the Object Models folder.",
                Fields =
                {
                    new HelpField("Model", "File",
                        "The 3D model for this object. Click ... to pick an FBX from the Object Models folder. If the file has animation, that is used too.",
                        "ModelBox", "LoadBtn"),
                    new HelpField("Blender FBX File", "On or off",
                        "Turn this on for models exported from Blender so animation size and the root bone line up. Turn it off for models from other programs.",
                        "BlenderBox"),
                    new HelpField("Name", "Text",
                        "The name of this object type, as it will appear when placing copies.",
                        "NameBox"),
                    new HelpField("Flag", "Number",
                        "A spare number on the type itself, not on a placed copy. Leave 0 unless a behavior needs it.",
                        "FlagBox"),
                    new HelpField("Scale", "Number",
                        "How large the model draws. 0.1 is a common starting size.",
                        "ScaleBox"),
                    new HelpField("Behavior", "Choice",
                        "What the object does (sit still, move, chase, and similar). Changing this fills in the parameter list below.",
                        "BehaviorBox"),
                    new HelpField("Parameters", "List",
                        "Extra numbers for the chosen behavior. Edit the Value column; the names come with the behavior.",
                        "ParameterView"),
                    new HelpField("Level Hitbox", "Number",
                        "How strongly this object rests on the course. 0 means it does not sit on the ground. 5 is a typical starting value.",
                        "LevelBump"),
                    new HelpField("Sound", "Choice",
                        "Which stock sound to play. The first entries (NONE and the global sounds) always play everywhere; Sound Type is locked to Global for those.",
                        "SoundNameBox"),
                    new HelpField("Sound Range", "Number",
                        "How close a player must be to hear a local sound. 250 is a typical starting value.",
                        "SoundRangeBox"),
                    new HelpField("Sound Type", "Choice",
                        "Global plays for everyone. Local only plays near the object. Locked to Global for the first sound names.",
                        "SoundTypeBox"),
                    new HelpField("Render Radius", "Number",
                        "How close a player must be before the object is drawn. Larger numbers keep it visible from farther away, at a cost. 700 is a typical starting value.",
                        "RenderBox"),
                    new HelpField("Face Camera", "On or off",
                        "When on, the object always turns to face the camera (good for trees and similar flat pictures).",
                        "CameraAlignBox"),
                    new HelpField("Gravity Enabled", "On or off",
                        "When on, the object falls with gravity. When off, it stays at the height you placed it.",
                        "GravityBox"),
                    new HelpField("Z-Sort [Slow]", "On or off",
                        "When on, the object is sorted more carefully against other see-through things. This is slower; leave it off unless the object draws in the wrong order.",
                        "ZSortBox"),
                    new HelpField("Hitbox list", "Choice",
                        "The collision shapes on this object. + adds a shape after the one you have selected. − removes the selected shape.",
                        "IndexBox", "AddBtn", "DeleteBtn"),
                    new HelpField("Type", "Choice",
                        "The shape of this hitbox.\r\n\r\n" +
                        "• Sphere: Size X only\r\n" +
                        "• Cylinder: Size X and Y\r\n" +
                        "• Box: Size X, Y, and Z, plus rotation around Z",
                        "TypeBox"),
                    new HelpField("Status", "Choice",
                        "What happens to a kart that hits this shape (spin, crash, and similar). None means no status change.",
                        "StatusBox"),
                    new HelpField("Effect", "Choice",
                        "The hit animation to play. None means no animation.",
                        "EffectBox"),
                    new HelpField("Solid Object", "On or off",
                        "When on, karts cannot drive through this shape.",
                        "solidBox"),
                    new HelpField("Hitbox Scale", "Number",
                        "Scale for this collision shape only. Separate from the model Scale above.",
                        "HitScaleBox"),
                    new HelpField("Pos. X/Y/Z", "Number",
                        "Where this hitbox sits relative to the object. The editor is Z-up: X and Y are the ground plane, Z is height.",
                        "OriginXBox", "OriginYBox", "OriginZBox"),
                    new HelpField("Size X/Y/Z", "Number",
                        "How large this hitbox is in editor space (Z-up). Unused axes are turned off for Sphere and Cylinder.",
                        "SizeXBox", "SizeYBox", "SizeZBox"),
                    new HelpField("Ang. Z", "Number",
                        "How the box is rotated around up (Z). Only used for Box.",
                        "AngleZBox"),
                    new HelpField("Save XML", "Button",
                        "Saves this object type (*.ok64.OBJECT) into the Object Models folder. If a model is already loaded from XML, you can save without picking the FBX again.",
                        "SaveXMLBtn"),
                    new HelpField("Load XML", "Button",
                        "Opens an object type from the Object Models folder and restores its fields, including sound, parameters, hitboxes, and the last model path.",
                        "LoadXMLBtn"),
                },
                SeeAlso = { "course.objects", "course.textures", "start.settings" }
            },
            new HelpTopic("tools.gamebuilder", "Game Builder", "tools")
            {
                Summary = "Tools → Game Builder puts compiled courses (*.ok64.Course) into a Mario Kart 64 (USA) ROM.\r\n\r\n" +
                    "Fill up to 4 courses in each cup, across 5 custom sets, then Build to write a new ROM.\r\n\r\n" +
                    "Leave Course Header Table at BE9178 unless you were told to change it for a special OverKart build.",
                Fields =
                {
                    new HelpField("Set Number", "Choice",
                        "Which custom cup set you are filling (Custom Set 1 through 5).",
                        "SetBox"),
                    new HelpField("Cup Number", "Choice",
                        "Which cup in that set: Mushroom, Flower, Star, Special, or Battle.\r\n\r\n" +
                        "Battle only accepts battle courses. Race cups reject battle courses.",
                        "CupBox"),
                    new HelpField("Course 1-4", "File",
                        "The four course slots for this cup.\r\n\r\n" +
                        "• Double-click: add a compiled course from the Course Models folder\r\n" +
                        "• Middle-click: clear the slot",
                        "CourseBox1", "CourseBox2", "CourseBox3", "CourseBox4"),
                    new HelpField("Course Header Table", "Number",
                        "Where OverKart looks up custom courses in the ROM.\r\n\r\n" +
                        "Leave this at BE9178 unless you were told to move it. A bad value shows \"Invalid Header Address\".",
                        "HeaderBox"),
                    new HelpField("Export Debug", "On or off",
                        "When on, Build also writes extra debug files next to the ROM (geometry pieces and the radar map). Most people can leave this off.",
                        "DebugBox"),
                    new HelpField("Expand", "Button",
                        "Shows or hides the header and debug options.",
                        "ExpandBtn"),
                    new HelpField("Save", "Button",
                        "Saves this cup layout so you can come back to it later.",
                        "SaveButton"),
                    new HelpField("Load", "Button",
                        "Opens a cup layout you saved earlier.",
                        "LoadButton"),
                    new HelpField("Build", "Button",
                        "Writes a ROM from the assigned courses.\r\n\r\n" +
                        "1. Pick a Mario Kart 64 (USA) ROM\r\n" +
                        "2. If it is not patched yet, the built-in patch is applied\r\n" +
                        "3. Each assigned course is inserted\r\n" +
                        "4. Save the new ROM",
                        "button2"),
                },
                SeeAlso = { "menus.file", "menus.patch", "start.settings", "course.settings" }
            },
        };

        public static HelpTopic GetTopic(string id)
        {
            if (id == null)
            {
                return null;
            }
            for (int i = 0; i < Topics.Count; i++)
            {
                if (Topics[i].Id == id)
                {
                    return Topics[i];
                }
            }
            return null;
        }

        public static List<string> GetDescendantIds(string rootTopicId)
        {
            List<string> ids = new List<string>();
            if (rootTopicId == null)
            {
                return ids;
            }
            ids.Add(rootTopicId);
            bool added = true;
            while (added)
            {
                added = false;
                for (int i = 0; i < Topics.Count; i++)
                {
                    HelpTopic topic = Topics[i];
                    if (topic.ParentId != null && ids.Contains(topic.ParentId) && !ids.Contains(topic.Id))
                    {
                        ids.Add(topic.Id);
                        added = true;
                    }
                }
            }
            return ids;
        }

        public static bool FindField(string rootTopicId, string controlName, out HelpTopic topic, out HelpField field)
        {
            topic = null;
            field = null;
            if (string.IsNullOrEmpty(rootTopicId) || string.IsNullOrEmpty(controlName))
            {
                return false;
            }
            List<string> ids = GetDescendantIds(rootTopicId);
            for (int i = 0; i < Topics.Count; i++)
            {
                HelpTopic candidate = Topics[i];
                if (!ids.Contains(candidate.Id) || candidate.Fields == null)
                {
                    continue;
                }
                for (int f = 0; f < candidate.Fields.Count; f++)
                {
                    HelpField candidateField = candidate.Fields[f];
                    if (candidateField.ControlNames == null)
                    {
                        continue;
                    }
                    for (int c = 0; c < candidateField.ControlNames.Length; c++)
                    {
                        if (candidateField.ControlNames[c] == controlName)
                        {
                            topic = candidate;
                            field = candidateField;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
