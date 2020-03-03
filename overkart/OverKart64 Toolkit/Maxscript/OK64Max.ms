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



rollout MK64Vimport "OverKart64 MAX" width:540 height:160

	(
		GroupBox grp1 "Setup" pos:[4,0] width:353 height:85

		edittext sectioncount pos:[185,20] width:160 height:25 readOnly:false
		button addSection "Setup Hierarchy" pos:[15,20] width:160 height:25

		edittext namtext pos:[185,50] width:160 height:25 readOnly:false
		button chgnam "Name" pos:[15,50] width:50 height:25
		button parnam "AutoPart" pos:[70,50] width:50 height:25
		button matnam "SwapMat" pos:[125,50] width:50 height:25

		GroupBox grp2 "Tools" pos:[4,85] width:530 height:65




		button clnbtn "Clone Objects" pos:[10,110] width:120 height:25

		button ASVbtn "Run Section-Views" pos:[140,110] width:120 height:25
		button prepBtn "Prep Course Model" pos:[270,110] width:120 height:25
		button spc2btn "Split Materials" pos:[400,110] width:120 height:25



		GroupBox grp4 "Paths + Objects" pos:[360,0] width:175 height:85
		button POPcreatebtn "Create POP Objects" pos:[365,20] width:160 height:25
		button POPexportbtn "Export POP Objects" pos:[365,50] width:160 height:25






		on MK64Vimport open do
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
				sectioncount.readOnly = true
				addSection.enabled = false
				section_count = sourceobj.Verts[1].pos.x
				if (section_count > 1) then
				(
					sectioncount.Text = section_count as String + " Sections"
				)
				else
				(
					sectioncount.Text = section_count as String + " Section"
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
				pathCount = 800

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
				topCount = 64

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
				topCount = 64

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

				searchstring = "Objects - Pirahna Plants"
				parentObj = getNodeByName searchstring exact:true

				countIndex = 1
				topCount = 64

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
			parentpath.pos = [0,0,-1500]
			parentpath.name = "Course Paths"




			marker_count = 800
			step_count = 10

			section_step = 800 / section_count
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
			pirahnaIndex = 1
			itemIndex = 1



			parentobj = undefined
			parentobj = box length:25 width:25 height:25
			parentobj.pos = [0,0,-1500]
			parentobj.name = "Objects - Trees"

			while treeIndex <= maxCount do
			(
				treeBlock = undefined
				treeBlock = box length:5 width:5 height:5
				treeBlock.pos = [0,(treeIndex * 10),0]
				treeBlock.name = (treeIndex as string)
				treeBlock.parent = parentobj
				treeIndex = treeIndex + 1
			)


			parentobj = undefined
			parentobj = box length:25 width:25 height:25
			parentobj.pos = [0,0,-1600]
			parentobj.name = "Objects - Pirahna Plants"

			while pirahnaIndex <= maxCount do
			(
				pirahnaBlock = undefined
				pirahnaBlock = box length:5 width:5 height:5
				pirahnaBlock.pos = [20,(pirahnaIndex * 10),0]
				pirahnaBlock.name = (pirahnaIndex as string)
				pirahnaBlock.parent = parentobj
				pirahnaIndex = pirahnaIndex + 1
			)

			parentobj = undefined
			parentobj = box length:25 width:25 height:25
			parentobj.pos = [0,0,-1700]
			parentobj.name = "Objects - Items Blocks"

			while itemIndex <= maxCount do
			(
				itemBlock = undefined
				itemBlock = box length:5 width:5 height:5
				itemBlock.pos = [40,(itemIndex * 10),0]
				itemBlock.name = (itemIndex as string)
				itemBlock.parent = parentobj
				itemIndex = itemIndex + 1
			)
			Print "Finished"
			MessageBox "Finished"
		)







		struct s_Node ( obj, dist )

		fn sortNodesFN node1 node2 =
		(
			if node1.dist < node2.dist then -1 else 1
		)






		on ASVbtn pressed do
		(


			start1 = timeStamp()
			searchstring = "Course Master Objects" as string
			format searchstring to:listener
			format "\n" to:listener
			masterobj = getNodeByName searchstring exact:true
			format masterobj.Name to:listener
			format "\n" to:listener



			for sectionview = 1 to section_count do
			(


				searchstring = "Section "+(sectionview as string)+" Surface" as string
				format searchstring to:listener
				format "\n" to:listener
				surfaceobj = getNodeByName searchstring exact:true
				format surfaceobj.Name to:listener
				format "\n" to:listener



				selectarray = #()

				print "raytest start"
				format "\n" to:listener



				for objectindex = 1 to surfaceobj.children.count do
				(


					printstring = "Start Object "+objectindex as string
					print printstring
					windows.processPostedMessages()
					tmesh = surfaceobj.children[objectindex]
					num_verts = tmesh.numverts
					num_faces = tmesh.numfaces

					-- sectionview NORTH
					for vertindex = 1 to num_verts do
					(
						vert = tmesh.verts[vertindex]



						printstring = "Section "+sectionview as string+"/"+section_count as string+" - Object "+objectindex as string +"/"+ surfaceobj.children.count as string +" - Vert "+vertindex as string + "/" + num_verts as string + " - North"
						print printstring
						windows.processPostedMessages()


						yval = vert.pos.y
						for child = 1 to masterobj.children.count do
						(
							childmesh = masterobj.children[child]
							child_verts = childmesh.numverts
							for childvertindex = 1 to child_verts do
							(
								vert = childmesh.verts[childvertindex]

								cyval = vert.pos.y

								if cyval > yval then
								(
									append selectarray childmesh
								)

							)

						)


					)

				)


				select selectarray

				print "north "+sectionview as string
				windows.processPostedMessages()
				searchstring = "Section "+(sectionview as string)+" North" as string
				sourceobj = getNodeByName searchstring exact:true
				if (sourceobj == undefined) then
				(
					specialsection = undefined
					specialsection = box length:15 width:15 height:15
					specialsection.name = searchstring
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
						nnl[1].isHidden = true

					)
				)




				selectarray = #()





				for objectindex = 1 to surfaceobj.children.count do
				(


					printstring = "Start Object "+objectindex as string
					print printstring
					windows.processPostedMessages()
					tmesh = surfaceobj.children[objectindex]
					num_verts = tmesh.numverts
					num_faces = tmesh.numfaces

					-- sectionview EAST
					for vertindex = 1 to num_verts do
					(
						vert = tmesh.verts[vertindex]



						printstring = "Section "+sectionview as string+"/"+section_count as string+" - Object "+objectindex as string +"/"+ surfaceobj.children.count as string +" - Vert "+vertindex as string + "/" + num_verts as string + " - East"
						print printstring
						windows.processPostedMessages()

						xval = vert.pos.x
						for child = 1 to masterobj.children.count do
						(
							childmesh = masterobj.children[child]
							child_verts = childmesh.numverts
							for childvertindex = 1 to child_verts do
							(
								vert = childmesh.verts[childvertindex]

								cxval = vert.pos.x

								if cxval > xval then
								(
									append selectarray childmesh
								)

							)

						)
					)
				)


				select selectarray

				print "east "+sectionview as string
				windows.processPostedMessages()
				searchstring = "Section "+(sectionview as string)+" East" as string
				sourceobj = getNodeByName searchstring exact:true
				if (sourceobj == undefined) then
				(
					specialsection = undefined
					specialsection = box length:15 width:15 height:15
					specialsection.name = searchstring
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
						nnl[1].isHidden = true

					)
				)



				selectarray = #()


				for objectindex = 1 to surfaceobj.children.count do
				(



					printstring = "Start Object "+objectindex as string
					print printstring
					windows.processPostedMessages()
					tmesh = surfaceobj.children[objectindex]
					num_verts = tmesh.numverts
					num_faces = tmesh.numfaces

					-- sectionview SOUTH
					for vertindex = 1 to num_verts do
					(
						vert = tmesh.verts[vertindex]


						printstring = "Section "+sectionview as string+"/"+section_count as string+" - Object "+objectindex as string +"/"+ surfaceobj.children.count as string +" - Vert "+vertindex as string + "/" + num_verts as string + " - South"
						print printstring
						windows.processPostedMessages()


						yval = vert.pos.y
						for child = 1 to masterobj.children.count do
						(
							childmesh = masterobj.children[child]
							child_verts = childmesh.numverts
							for childvertindex = 1 to child_verts do
							(
								vert = childmesh.verts[childvertindex]

								cyval = vert.pos.y

								if cyval < yval then
								(
									append selectarray childmesh
								)

							)

						)
					)
				)


				select selectarray

				print "south "+sectionview as string
				windows.processPostedMessages()
				searchstring = "Section "+(sectionview as string)+" South" as string
				sourceobj = getNodeByName searchstring exact:true
				if (sourceobj == undefined) then
				(
					specialsection = undefined
					specialsection = box length:15 width:15 height:15
					specialsection.name = searchstring
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
						nnl[1].isHidden = true

					)
				)



				selectarray = #()


				for objectindex = 1 to surfaceobj.children.count do
				(



					printstring = "Start Object "+objectindex as string
					print printstring
					windows.processPostedMessages()
					tmesh = surfaceobj.children[objectindex]
					num_verts = tmesh.numverts
					num_faces = tmesh.numfaces


					-- sectionview WEST
					for vertindex = 1 to num_verts do
					(
						vert = tmesh.verts[vertindex]

						printstring = "Section "+sectionview as string+"/"+section_count as string+" - Object "+objectindex as string +"/"+ surfaceobj.children.count as string +" - Vert "+vertindex as string + "/" + num_verts as string + " - West"
						print printstring
						windows.processPostedMessages()

						xval = vert.pos.x
						for child = 1 to masterobj.children.count do
						(n
							childmesh = masterobj.children[child]
							child_verts = childmesh.numverts
							for childvertindex = 1 to child_verts do
							(
								vert = childmesh.verts[childvertindex]

								cxval = vert.pos.x

								if cxval < xval then
								(
									append selectarray childmesh
								)

							)

						)


					)
				)


				select selectarray
				print "west "+sectionview as string
				windows.processPostedMessages()
				searchstring = "Section "+(sectionview as string)+" West" as string
				sourceobj = getNodeByName searchstring exact:true
				if (sourceobj == undefined) then
				(
					specialsection = undefined
					specialsection = box length:15 width:15 height:15
					specialsection.name = searchstring
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
						nnl[1].isHidden = true

					)
				)



				selectarray = #()



				printstring = "raytest end " + sectionview as string + " section"
				print printstring
			)
				end1 = timeStamp()
				time = (end1 - start1) / 1000

				minutes = 0

				while time > 60 do
				(
					time = time - 60
					minutes = minutes + 1
				)

				printstring = "Took " + minutes as string+" minutes and "+time as string + " seconds."
				MessageBox printstring


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








		on spCBtn pressed do
		(


			undo "MK64 Split" on
			(

				z = 1



				searchstring = "Special Section Surface"
				format searchstring to:listener


				obj = getNodeByName searchstring exact:true
				format obj.Name to:listener
				select obj.children


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

					if (theDetachedObj.numfaces > 25) do
					(
						try(convertToPoly theDetachedObj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
						facecount = theDetachedObj.numfaces

						the_name = "name"
						faces_left = facecount
						i = 1

						while (faces_left > 0) do
						(


							face_array = #()

							if (faces_left > 25) then
							(
								for f = 1 to 25 do
								(
									append face_array f
								)
								setFaceSelection theDetachedObj face_array
								the_faces = getFaceSelection theDetachedObj
								polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
								newDetach = (getnodebyname the_name)
								newDetach.parent = theDetachedObj.parent
								newDetach.name = theDetachedObj.name
								faces_left = faces_left - 25
							)
							else
							(
								for f = 1 to faces_left do
								(
									append face_array f
								)
								setFaceSelection theDetachedObj face_array
								the_faces = getFaceSelection theDetachedObj
								polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
								newDetach = (getnodebyname the_name)
								newDetach.parent = theDetachedObj.parent
								newDetach.name = theDetachedObj.name
								faces_left = 0
							)
							i = i +1
						)
					)


				)


				while z <= section_count do
				(

					searchstring = "Section "+(z) as string
					format searchstring to:listener
					sourceobj = getNodeByName searchstring exact:true
					format sourceobj.Name to:listener

					sourcechild = undefined

					x = 5

					obj = sourceobj.children[x]
					select obj.children


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


						if (theDetachedObj.numfaces > 25) do
						(
							try(convertToPoly theDetachedObj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
							facecount = theDetachedObj.numfaces

							the_name = "name"
							faces_left = facecount
							i = 1

							while (faces_left > 0) do
							(


								face_array = #()

								if (faces_left > 25) then
								(
									for f = 1 to 25 do
									(
										append face_array f
									)
									setFaceSelection theDetachedObj face_array
									the_faces = getFaceSelection theDetachedObj
									polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
									newDetach = (getnodebyname the_name)
									newDetach.parent = theDetachedObj.parent
									newDetach.name = theDetachedObj.name
									faces_left = faces_left - 25
								)
								else
								(
									for f = 1 to faces_left do
									(
										append face_array f
									)
									setFaceSelection theDetachedObj face_array
									the_faces = getFaceSelection theDetachedObj
									polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
									newDetach = (getnodebyname the_name)
									newDetach.parent = theDetachedObj.parent
									newDetach.name = theDetachedObj.name
									faces_left = 0
								)
								i = i +1
							)
						)

						x=x+1
					)
					z = z + 1
				)
			)

		)







		on spCBtn pressed do
		(


			undo "MK64 Split" on
			(

				z = 1



				searchstring = "Special Section Surface"
				format searchstring to:listener


				obj = getNodeByName searchstring exact:true
				format obj.Name to:listener
				select obj.children


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

					if (theDetachedObj.numfaces > 25) do
					(
						try(convertToPoly theDetachedObj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
						facecount = theDetachedObj.numfaces

						the_name = "name"
						faces_left = facecount
						i = 1

						while (faces_left > 0) do
						(


							face_array = #()

							if (faces_left > 25) then
							(
								for f = 1 to 25 do
								(
									append face_array f
								)
								setFaceSelection theDetachedObj face_array
								the_faces = getFaceSelection theDetachedObj
								polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
								newDetach = (getnodebyname the_name)
								newDetach.parent = theDetachedObj.parent
								newDetach.name = theDetachedObj.name
								faces_left = faces_left - 25
							)
							else
							(
								for f = 1 to faces_left do
								(
									append face_array f
								)
								setFaceSelection theDetachedObj face_array
								the_faces = getFaceSelection theDetachedObj
								polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
								newDetach = (getnodebyname the_name)
								newDetach.parent = theDetachedObj.parent
								newDetach.name = theDetachedObj.name
								faces_left = 0
							)
							i = i +1
						)
					)


				)


				while z <= section_count do
				(

					searchstring = "Section "+(z) as string
					format searchstring to:listener
					sourceobj = getNodeByName searchstring exact:true
					format sourceobj.Name to:listener

					sourcechild = undefined

					x = 5

					obj = sourceobj.children[x]
					select obj.children


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


						if (theDetachedObj.numfaces > 25) do
						(
							try(convertToPoly theDetachedObj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
							facecount = theDetachedObj.numfaces

							the_name = "name"
							faces_left = facecount
							i = 1

							while (faces_left > 0) do
							(


								face_array = #()

								if (faces_left > 25) then
								(
									for f = 1 to 25 do
									(
										append face_array f
									)
									setFaceSelection theDetachedObj face_array
									the_faces = getFaceSelection theDetachedObj
									polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
									newDetach = (getnodebyname the_name)
									newDetach.parent = theDetachedObj.parent
									newDetach.name = theDetachedObj.name
									faces_left = faces_left - 25
								)
								else
								(
									for f = 1 to faces_left do
									(
										append face_array f
									)
									setFaceSelection theDetachedObj face_array
									the_faces = getFaceSelection theDetachedObj
									polyOp.detachFaces theDetachedObj the_faces asnode:true name:the_name
									newDetach = (getnodebyname the_name)
									newDetach.parent = theDetachedObj.parent
									newDetach.name = theDetachedObj.name
									faces_left = 0
								)
								i = i +1
							)
						)

						x=x+1
					)
					z = z + 1
				)
			)

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
			specialsection.name = "Special Objects"
			specialsection.pos=[ -50 , -1250 , -1500]

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
				nextsection.pos=[section_count*50 , -1000 , -1500]

				childsection = undefined
				childsection = box length:15 width:15 height:15
				childsection.name = "Section "+i as string+" North"
				childsection.pos=[section_count*50 , -1050 , -1500]
				childsection.parent = nextsection

				childsection = undefined
				childsection = box length:15 width:15 height:15
				childsection.name = "Section "+i as string+" East"
				childsection.pos=[section_count*50 , -1100 , -1500]
				childsection.parent = nextsection

				childsection = undefined
				childsection = box length:15 width:15 height:15
				childsection.name = "Section "+i as string+" South"
				childsection.pos=[section_count*50 , -1150 , -1500]
				childsection.parent = nextsection

				childsection = undefined
				childsection = box length:15 width:15 height:15
				childsection.name = "Section "+i as string+" West"
				childsection.pos=[section_count*50 , -1200 , -1500]
				childsection.parent = nextsection

				childsection = undefined
				childsection = box length:15 width:15 height:15
				childsection.name = "Section "+i as string+" Surface"
				childsection.pos=[section_count*50 , -1250 , -1500]
				childsection.parent = nextsection

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


















		on exRbtn pressed do
		(

			output_name = getSaveFileName caption:"Exporting Section View" \
			filename: "" \
			types:"TXT (*.txt)|*.txt|All Files (*.*)|*.*|"


			out_file = createfile output_name


			material_read = 0
			printstring = section_count as string

			format printstring to: listener
			format "\n" to: listener
			format printstring to: out_file
			format "\n" to: out_file
			i = 1
			while i <= section_count do
			(

				searchstring = "Section "+(i) as string
				sourceobj = getNodeByName searchstring exact:true


				sourcechild = undefined




				x = 1
				while x <= 4 do
				(
					sourcechild = sourceobj.children[x]


					printstring = sourcechild.name
					format printstring to:listener
					format "\n" to:listener
					format printstring to: out_file
					format "\n" to: out_file



					printstring = sourcechild.children.count as string
					format printstring to: listener
					format "\n" to: listener
					format printstring to: out_file
					format "\n" to: out_file

					select sourcechild.children

					n = 1
					while n <= sourcechild.children.count do
					(
						printstring = sourcechild.children[n].name
						format printstring to: listener
						format "\n" to: listener
						format printstring to: out_file
						format "\n" to: out_file

						n = n+1
					)
					x = x + 1

				)
				i=i+1
			)
			close out_file
			messagebox "Finished"

		)









		on imRbtn pressed do
		(

			in_name = getOpenFileName \
					filename: "" \
					types: "TXT File (*.txt)|*.txt|All Files (*.*)|*.*"
			if in_name != undefined then
			(

				in_file = openfile in_name


				material_read = 0
				printstring = section_count as string



				format printstring to: listener
				format "\n" to: listener

				i = 1

				readstring = readLine in_file as string
				filesection_count = readstring as integer


				printstring = filesection_count as string



				format printstring to: listener
				format "\n" to: listener

				while i <= filesection_count do
				(

					SectionName_Garbage = readLine in_file as string

					readstring = readLine in_file as string
					subsection_count = readstring as integer
					n = 1



					printstring = i as string



					format printstring to: listener
					format "\n" to: listener



					while n<= subsection_count do
					(
						searchstring = readLine in_file as string
						childobj = getNodeByName searchstring exact:true

						if (childobj == undefined) then
						(
						printstring = "UNDEFINED" as string + searchstring
						format printstring to: listener
						format "\n" to: listener
						)
						searchstring = "Section "+(i) as string+" North"
						parentobj = getNodeByName searchstring exact:true


						maxOps.cloneNodes childobj cloneType:#copy newNodes:&nnl #nodialog
						nnl[1].name = childobj.name
						nnl[1].parent = parentobj

						n = n+1

					)

					SectionName_Garbage = readLine in_file as string
					format SectionName_Garbage to: listener
					format "\n" to: listener



					readstring = readLine in_file as string
					subsection_count = readstring as integer
					n = 1

					while n <= subsection_count do
					(
						searchstring = readLine in_file as string
						childobj = getNodeByName searchstring exact:true

						searchstring = "Section "+(i) as string+" East"
						parentobj = getNodeByName searchstring exact:true


						maxOps.cloneNodes childobj cloneType:#copy newNodes:&nnl #nodialog
						nnl[1].name = childobj.name
						nnl[1].parent = parentobj

						n = n+1
					)

					SectionName_Garbage = readLine in_file as string
					format SectionName_Garbage to: listener
					format "\n" to: listener

					readstring = readLine in_file as string
					subsection_count = readstring as integer
					n = 1

					while n<= subsection_count do
					(
						searchstring = readLine in_file as string
						childobj = getNodeByName searchstring exact:true

						searchstring = "Section "+(i) as string+" South"
						parentobj = getNodeByName searchstring exact:true


						maxOps.cloneNodes childobj cloneType:#copy newNodes:&nnl #nodialog
						nnl[1].name = childobj.name
						nnl[1].parent = parentobj

						n = n+1
					)

					SectionName_Garbage = readLine in_file as string
					format SectionName_Garbage to: listener
					format "\n" to: listener

					readstring = readLine in_file as string
					subsection_count = readstring as integer
					n = 1

					while n<= subsection_count do
					(
						searchstring = readLine in_file as string
						childobj = getNodeByName searchstring exact:true

						searchstring = "Section "+(i) as string+" West"
						parentobj = getNodeByName searchstring exact:true


						maxOps.cloneNodes childobj cloneType:#instance newNodes:&nnl #nodialog
						nnl[1].name = childobj.name
						nnl[1].parent = parentobj

						n = n+1
					)
					printstring = "Done"



					format printstring to: listener
					format "\n" to: listener
					i = i + 1
				)
				close in_file
				messagebox "Finished"
			)

		)

		-- End of rollout
	)
CreateDialog MK64Vimport
