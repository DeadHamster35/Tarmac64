# CREDITS TO ROBICHU

import bpy
import os

currPath = os.path.splitext(bpy.data.filepath)[0]+".OK64.POP"
file = open(currPath, "w")
ob = bpy.context.object # active object

# Scale. Try setting to zero if POP objects are too large.
# This line scales Blender FBX files to 3DSMax sized objects
scale = 100

# Disable this to prevent creating missing collections
# May create errors
autoPopulate = True

# If you would like to customize the names in your scene collections,
# modify these variables
sections_name = "Section"
master_name = "Course Master Objects"
path_name = "Path"
items_name = "Itemboxes"
trees_name = "Trees"
piranhas_name = "Piranhas"

# Sets blender into object mode.
bpy.ops.object.mode_set(mode="OBJECT")

# Get collections or create them if they do not exist
sections = None
master = None
path = None
items = None
trees = None
piranhas = None
cancel = False
for cc in bpy.data.collections:
    if cc.name.startswith(sections_name):
        if sections == None:
            sections = []
        sections.append(cc)
        
    if cc.name.startswith(master_name):
        master = cc.all_objects
    
    if cc.name.startswith(path_name):
        path = cc.objects[0].data.splines.active.bezier_points
    
    if cc.name.startswith(items_name):
        items = cc.all_objects
            
    if cc.name.startswith(trees_name):
        trees = cc.all_objects
            
    if cc.name.startswith(piranhas_name):
        piranhas = cc.all_objects

if sections == None:
    sec = bpy.data.collections.new(sections_name+" 1")
    bpy.context.scene.collection.children.link(sec)
    sections = []
    sections.append(sec)
    print(sections_name+" collection created please add to it an object to continue.")
    
if master == None and autoPopulate == True:
    mas = bpy.data.collections.new(master_name)
    bpy.context.scene.collection.children.link(mas)
    master = mas.all_objects
    print(master_name+" collection created.")

if path == None and autoPopulate == True:
    pat = bpy.data.collections.new(path_name)
    bpy.context.scene.collection.children.link(pat)
    layer_collection = bpy.context.view_layer.layer_collection.children[pat.name]
    bpy.context.view_layer.active_layer_collection = layer_collection
    bpy.ops.curve.primitive_bezier_curve_add(radius=1.0,
                                      location=(0.0, 0.0, 0.0),
                                      enter_editmode=False)
    path = pat.objects[0].data.splines.active.bezier_points
    print(path_name+" collection created.")

if items == None and autoPopulate == True:
    ite = bpy.data.collections.new(items_name)
    bpy.context.scene.collection.children.link(ite)
    items = ite.all_objects
    print(items_name+" collection created.")
    
if trees == None and autoPopulate == True:
    tre = bpy.data.collections.new(trees_name)
    bpy.context.scene.collection.children.link(tre)
    trees = tre.all_objects
    print(trees_name+" collection created.")
    
if piranhas == None and autoPopulate == True:
    pir = bpy.data.collections.new(piranhas_name)
    bpy.context.scene.collection.children.link(pir)
    piranhas = pir.all_objects
    print(piranhas_name+" collection created.")


# Write functions

# Write section objects.
def sectionObjects():
    for se in sections:
        # To lowercase and removes white-spaces from section names.
        file.write(se.name.lower().replace(" ", ""))
        file.write("\n")
        file.write(str(len(se.objects)))
        file.write("\n")
        file.write("[")
        for part in se.objects:
            file.write(part.name)
            if part is not se.objects[-1]:
                file.write(", ")
        file.write("]")
        file.write("\n")

# Write course master Objects
def masterObjects():
    file.write("[")
    for m in master:
        file.write(m.name)
        if m is not master[-1]:
            file.write(", ")
    file.write("]")
    file.write("\n")

# Write path object
def pathObject():
    for p in path:
        file.write("[")
        file.write(str((p.co.x * scale))+",")
        file.write(str((p.co.y * scale))+",")
        file.write(str((p.co.z * scale)))
        file.write("]")
        file.write('\n')
        file.write("1")
        file.write("\n")

# Write item objects
def itemObjects():
    for b in items:
        file.write("[")
        file.write(str((b.location.x * scale))+",")
        file.write(str((b.location.y * scale))+",")
        file.write(str((b.location.z * scale)))
        file.write("]")
        file.write('\n')
        file.write("1")
        file.write("\n")

def treeObjects():
    for f in trees:
        file.write("[")
        file.write(str((f.location.x * scale))+",")
        file.write(str((f.location.y * scale))+",")
        file.write(str((f.location.z * scale)))
        file.write("]")
        file.write('\n')
        file.write("1")
        file.write("\n")

def piranhaObjects():
    for d in piranhas:
        file.write("[")
        file.write(str((d.location.x * scale))+",")
        file.write(str((d.location.y * scale))+",")
        file.write(str((d.location.z * scale)))
        file.write("]")
        file.write('\n')
        file.write("1")
        file.write("\n")




# Main loop

# sectionObjects
if sections:
    sectionObjects()

# masterObjects
file.write("course_master_objects")
file.write('\n')
file.write(str(len(master)))
file.write('\n')
if master:
    masterObjects()

# pathObject
file.write("path")
file.write('\n')
file.write(str(len(path)))
file.write('\n')
if path:
    pathObject()

# itemObjects    
file.write("item")
file.write('\n')
file.write(str(len(items)))
file.write('\n')
if items:
    itemObjects()

# treeObjects
file.write("tree")
file.write('\n')
file.write(str(len(trees)))
file.write('\n')
if trees:
    treeObjects()

# piranhaObjects
file.write("piranha")
file.write('\n')
file.write(str(len(piranhas)))
file.write('\n')
if piranhas:
    piranhaObjects()
    
print("Write OK!")
print("File location:"+currPath)
file.close()
# eof
