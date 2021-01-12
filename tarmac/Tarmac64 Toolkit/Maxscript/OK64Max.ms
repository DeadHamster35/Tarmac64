file=""
file_name=""

maxver = maxVersion()

----------------------------------------------------------------------------------------------------
-- MK64 Course Exporter
-- <3 hamp
----------------------------------------------------------------------------------------------------

Global x = 0
Global i = 0
Global sourcechild = none
Global output_name = ""
Global section_count = 0

Global line_count = 0

struct vertex
(
pos,
tex
)



rollout OverKartMAX "OverKart64 MAX" width:1000 height:45

(
GroupBox grp1 "Setup" pos:[0,0] width:170 height:45

edittext sectioncount pos:[85,20] width:75 height:20 readOnly:false
button addSection "Create" pos:[5,20] width:75 height:20

label author "<3 Hamp" pos:[940, 5] enabled:false

GroupBox grp2 "Tools" pos:[175,0] width:245 height:45
button clnbtn "Clone Objects" pos:[180,20] width:75 height:20
button prepBtn "Prep Model" pos:[260,20] width:75 height:20
button spc2btn "Split Material" pos:[340,20] width:75 height:20



GroupBox grp4 "Paths + Objects" pos:[425,0] width:165 height:45
button POPcreatebtn "Create" pos:[430,20] width:75 height:20
button POPexportbtn "Export" pos:[510,20] width:75 height:20

GroupBox grp5 "Materials and Names" pos:[595,0] width:340 height:45
edittext namtext pos:[600,20] width:160 height:20 readOnly:false
button chgnam "Name" pos:[770,20] width:50 height:20
button parnam "AutoPart" pos:[825,20] width:50 height:20
button matnam "SwapMat" pos:[880,20] width:50 height:20





on OverKartMAX open do
(

ClearListener()
temp_buffer = fopen (sysInfo.tempdir + "\\float_conv_buffer.txt") "wb+"

searchstring = "section count"
sourceobj = getNodeByName searchstring exact:true
if (sourceobj == undefined) then
(
section_count = 0

)
else
(

section_count = sourceobj.Verts[1].pos.x
if (section_count > 1) then
(
sectioncount.Text = (section_count as Integer) as String
)
else
(
sectioncount.Text = (section_count as Integer) as String
)
)
)
















on POPexportbtn pressed do
(

output_name = getSaveFileName caption:"Exporting Path and Object Positions" \
filename: "" \
types:"OK64-POP (*.OK64.POP)|*.OK64.POP|All Files (*.*)|*.*|"

if output_name == undefined then
(

)
else
(
txt = createfile output_name


searchstring = "Course Paths"
parentObj = getNodeByName searchstring exact:true
pathObj = parentObj.Children[1]

pathIndex = 1
pathCount = numKnots pathObj 1

printstring = "path"
format printstring to:txt
format "\n" to:txt

printstring = pathCount as string
format printstring to:txt
format "\n" to:txt

while pathIndex <= pathCount do
(
vertPos = getKnotPoint pathObj 1 pathIndex
printstring = vertPos as string
format printstring to:txt
format "\n" to:txt

vertFlag = materialID pathObj 1 pathIndex
printstring = vertFlag as string
format printstring to:txt
format "\n" to:txt

pathIndex = pathIndex+1
)



searchstring = "Objects - Items Blocks"
parentObj = getNodeByName searchstring exact:true

countIndex = 1
topCount = parentObj.Children.Count

printstring = "item"
format printstring to:txt
format "\n" to:txt


printstring = topCount as string
format printstring to:txt
format "\n" to:txt

while countIndex <= topCount do
(
vertPos = parentObj.Children[countIndex].pos
printstring = vertPos as string
format printstring to:txt
format "\n" to:txt

vertFlag = 0
printstring = vertFlag as string
format printstring to:txt
format "\n" to:txt

countIndex = countIndex+1
)

searchstring = "Objects - Trees"
parentObj = getNodeByName searchstring exact:true

countIndex = 1
topCount = parentObj.Children.Count


printstring = "tree"
format printstring to:txt
format "\n" to:txt


printstring = topCount as string
format printstring to:txt
format "\n" to:txt

while countIndex <= topCount do
(
vertPos = parentObj.Children[countIndex].pos
printstring = vertPos as string
format printstring to:txt
format "\n" to:txt

vertFlag = 0
printstring = vertFlag as string
format printstring to:txt
format "\n" to:txt


countIndex = countIndex+1
)

searchstring = "Objects - Piranha Plants"
parentObj = getNodeByName searchstring exact:true

countIndex = 1
topCount = parentObj.Children.Count


printstring = "plant"
format printstring to:txt
format "\n" to:txt


printstring = topCount as string
format printstring to:txt
format "\n" to:txt

while countIndex <= topCount do
(
vertPos = parentObj.Children[countIndex].pos
printstring = vertPos as string
format printstring to:txt
format "\n" to:txt

vertFlag = 0
printstring = vertFlag as string
format printstring to:txt
format "\n" to:txt


countIndex = countIndex+1
)


searchstring = "Objects - Red Coins"
parentObj = getNodeByName searchstring exact:true

countIndex = 1
topCount = parentObj.Children.Count


printstring = "redcoin"
format printstring to:txt
format "\n" to:txt


printstring = topCount as string
format printstring to:txt
format "\n" to:txt

while countIndex <= topCount do
(
vertPos = parentObj.Children[countIndex].pos
printstring = vertPos as string
format printstring to:txt
format "\n" to:txt

vertFlag = 0
printstring = vertFlag as string
format printstring to:txt
format "\n" to:txt


countIndex = countIndex+1
)



MessageBox "Finished"
close txt

)


)


fn drawLineBetweenTwoPoints pointA pointB =
(
ss = SplineShape pos:pointA
addNewSpline ss
addKnot ss 1 #corner #line PointA
addKnot ss 1 #corner #line PointB
updateShape ss
ss
)




on POPcreatebtn pressed do
(
Print "Startup Paths"


parentpath = undefined
parentpath = box length:25 width:25 height:25
parentpath.pos = [100,-50,-1500]
parentpath.name = "Course Paths"




marker_count = 800
step_count = 10
if (section_count > 0) then
(
section_step = 800 / section_count
)
else
section_step = 800
current_section = 1

print ("Section Step -"+(section_step as string))

lastX = 0
lastY = 0
lastZ = 0
thisX = 0
thisY = 0
thisZ = 0
marker = 1


master_spline = undefined

stop_count = marker_count / 8

while marker <= marker_count do
(
print marker

if ((marker - (section_step * (current_section - 1))) >= section_step) then
(
if (current_section < section_count) then
(
current_section = current_section + 1
windows.processPostedMessages()
)
)

if (marker <= stop_count) then
(
thisX = thisX + step_count
thisY = thisY
thisZ = thisZ
)
if ((marker > stop_count) and (marker <= stop_count * 3)) then
(
thisX = thisX
thisY = thisY + step_count
thisZ = thisZ
)
if ((marker > stop_count * 3) and (marker <= stop_count * 5)) then
(
thisX = thisX - step_count
thisY = thisY
thisZ = thisZ
)
if ((marker > stop_count * 5) and (marker <= stop_count * 7)) then
(
thisX = thisX
thisY = thisY - step_count
thisZ = thisZ
)
if ((marker > stop_count * 7) and (marker <= stop_count * 8)) then
(
thisX = thisX + step_count
thisY = thisY
thisZ = thisZ
)


newSpline = undefined
newSpline = drawLineBetweenTwoPoints [lastX,lastY,lastZ] [thisX,thisY,thisZ]
newSpline.parent = parentpath


setMaterialID newSpline 1 1 current_section

if (marker == 1) then
(
masterSpline = newSpline
)
else
(
addAndWeld masterSpline newSpline 0
)

lastX = thisX
lastY = thisY
lastZ = thisZ

marker = marker + 1


)


Print "Startup Objects"




maxCount = 64

treeIndex = 1
piranhaIndex = 1
itemIndex = 1



parentobj = undefined
parentobj = box length:25 width:25 height:25
parentobj.pos = [100,0,-1500]
parentobj.name = "Objects - Trees"


treeBlock = undefined
treeBlock = box length:5 width:5 height:5
treeBlock.pos = [0,0,0]
treeBlock.name = "tree"
treeBlock.parent = parentobj




parentobj = undefined
parentobj = box length:25 width:25 height:25
parentobj.pos = [100,50,-1500]
parentobj.name = "Objects - Piranha Plants"


piranhaBlock = undefined
piranhaBlock = box length:5 width:5 height:5
piranhaBlock.pos = [20,0,0]
piranhaBlock.name = "Piranha"
piranhaBlock.parent = parentobj



parentobj = undefined
parentobj = box length:25 width:25 height:25
parentobj.pos = [100,100,-1700]
parentobj.name = "Objects - Items Blocks"


itemBlock = undefined
itemBlock = box length:5 width:5 height:5
itemBlock.pos = [40,0,0]
itemBlock.name = "Item"
itemBlock.parent = parentobj

parentobj = undefined
parentobj = box length:25 width:25 height:25
parentobj.pos = [100,0,-1500]
parentobj.name = "Objects - Red Coins"


treeBlock = undefined
treeBlock = box length:5 width:5 height:5
treeBlock.pos = [0,0,0]
treeBlock.name = "Coin 1"
treeBlock.parent = parentobj


Print "Finished"
MessageBox "Finished"
)







struct s_Node ( obj, dist )

fn sortNodesFN node1 node2 =
(
if node1.dist < node2.dist then -1 else 1
)


fn getMaterialIDs obj =
(
id_arr = #()

for i in 1 to 257 do
(
obj.selectByMaterial i
the_faces = getFaceSelection obj
if (the_faces as array).count != 0 do
(
append id_arr i
)
)
id_arr
)

on chgnam pressed do
(
the_sel = selection as array
if (the_sel.count != 0) do
(
i = 0
for obj in the_sel do
(
printstring = namtext.Text
obj.name = printstring
i=i+1
)
)
messagebox "Finished"
)

on parnam pressed do
(
the_sel = selection as array
if (the_sel.count != 0) do
(
i = 0
for obj in the_sel do
(
printstring = namtext.Text+"_part"+i as string
obj.name = printstring
i=i+1
)
)
messagebox "Finished"
)

on matnam pressed do
(
the_sel = selection as array
if (the_sel.count != 0) do
(
i = 0
for obj in the_sel do
(
printstring = filterString obj.name "_"
sendstring = namtext.Text+"_"+printstring[2]
format sendstring to:listener
format "\n" to:listener
i=i+1
obj.name = sendstring
)
)
messagebox "Finished"
)




on spC2Btn pressed do
(


undo "MK64 Split" on
(


the_sel = selection as array
for obj in the_sel where \
superclassof obj == GeometryClass and \
obj.material != undefined do
(
try(convertToPoly obj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
the_orig = obj
the_polyobj = copy the_orig

theDetachedObj = ()

the_mat = obj.material
id_count = the_mat.numSubs
id_list = getMaterialIDs the_polyobj

-- start detaching
for i in id_list do
(
the_polyobj.selectByMaterial i
the_faces = getFaceSelection the_polyobj

the_name = "name"


polyOp.detachFaces the_polyobj the_faces asnode:true name:the_name
theDetachedObj = (getnodebyname the_name)


theDetachedObj.parent = the_orig.parent
convertToMesh theDetachedObj
convert = getFaceMatID theDetachedObj 1

finalname = convert as string

theDetachedObj.name = finalname
)-- end detach loop
delete the_polyobj
delete the_orig




)
)
)










on clnbtn pressed do
(

searchstring = "Temp Section"
sourceobj = getNodeByName searchstring exact:true
if (sourceobj == undefined) then
(
specialsection = undefined
specialsection = box length:15 width:15 height:15
specialsection.name = "Temp Section"
specialsection.pos=[ -50 , -1250 , -1500]
sourceobj = specialsection
)
the_sel = selection as array
if (the_sel.count != 0) do
(

for obj in the_sel do
(
maxOps.cloneNodes obj cloneType:#copy newNodes:&nnl #nodialog
nnl[1].name = obj.name
nnl[1].parent = sourceobj
)
)
messagebox "Finished"

)














on prepBtn pressed do
(

undo "MK64 Prep" on
(


searchstring = "Master Course"
sourceobj = getNodeByName searchstring exact:true
format sourceobj.Name to:listener


tmesh = sourceobj

num_verts = tmesh.numverts


for v = 1 to num_verts do
(
vert = tmesh.verts[v]
vert.pos.x = vert.pos.x as integer
vert.pos.y = vert.pos.y as integer
vert.pos.z = vert.pos.z as integer
)

messagebox "Finished"

)-- end Undo
)















on addSection pressed do
(
section_count = sectioncount.Text as integer
Print "Adding Section - " + section_count as string


specialsection = undefined
specialsection = box length:15 width:15 height:15
specialsection.name = "Course Master Objects"
specialsection.pos=[ -50 , -1000 , -1500]


sectioncount.Text =""


i = 1
while i <= section_count do
(
nextsection = undefined
nextsection = box length:25 width:25 height:25
nextsection.name = "Section "+i as string
nextsection.pos=[i*50 , -1000 , -1500]

i = i+1
)



searchstring = "section count"
sourceobj = getNodeByName searchstring exact:true
if (sourceobj == undefined) then
(
v1 = [section_count,75,-1550]
v2 = [section_count,0,-1550]
v3 = [section_count,0,-1575]
vert_array = #()
face_array = #()
append vert_array v1
append vert_array v2
append vert_array v3
sourceobj = mesh vertices:vert_array faces:#([1,2,3])
sourceobj.name = "section count"
)



)



)

CreateDialog OverKartMAX
cui.RegisterDialogBar OverKartMAX style:#(#cui_dock_bottom, #cui_floatable)
cui.DockDialogBar OverKartMAX #cui_dock_bottom
