file=""
file_name=""

maxver = maxVersion()

----------------------------------------------------------------------------------------------------
-- MK64 Path Importer
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



rollout MK64Vimport "OverKart64 MAX" width:180 height:290
	(	
		GroupBox grp3 "Sections" pos:[3,0] width:175 height:200
		
		edittext sectioncount pos:[10,20] width:160 height:25 readOnly:true
		button addSection "Add New Section" pos:[10,50] width:160 height:25
		button expBtn "Export" pos:[10,80] width:160 height:25
		button colBtn "Export Collision" pos:[10,110] width:160 height:25
		button preBtn "Prep Verts" pos:[10,140] width:160 height:25
		button splBtn "Split Faces" pos:[10,170] width:160 height:25
		
		GroupBox nam3 "Name Changer" pos:[3,205] width:175 height:80
		
		edittext namtext pos:[10,225] width:160 height:25 readOnly:false
		button chgnam "Change Name" pos:[10,255] width:160 height:25
		
		
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
				for obj in the_sel do			
				(
					obj.name = namtext.Text
				)
			)	
			messagebox "Finished"
		)
		
		
		
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
				section_count = sourceobj.pos.x as Integer
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
		
		
		
		
		on splBtn pressed do
		
		(
		
			undo "MK64 Prep" on
			(
				the_sel = selection as array
				if the_sel.count != 0 do
				(
					for obj in the_sel where \
					superclassof obj == GeometryClass and \
					obj.material != undefined do
					(
						try(convertToPoly obj)catch(exit; print "DetacByID: Error Occured on Object Conversion")
						the_orig = obj
						the_polyobj = copy the_orig
						
						
						
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
							
							theDetachedObj.name = convert as string
						)-- end detach loop
						delete the_polyobj
						delete the_orig
					)-- end object check loop
				)-- end selection loop
			)
			
		)
		
		
		on preBtn pressed do
		(
		
			undo "MK64 Prep" on
			(
				
				i = 1
			
			
				while i <= section_count do
				(
				
					searchstring = "Section "+(i-1) as string
					sourceobj = getNodeByName searchstring exact:true
					format sourceobj.Name to:listener
					
					sourcechild = undefined
					
					x = 1
					while x <= 4 do
					(
						sourcechild = sourceobj.children[x]
						
						
						n = 1
						while n <= sourcechild.children.count do
						(	
							tmesh = sourcechild.children[n]
							convert = 0
							num_verts = tmesh.numverts
							num_faces = tmesh.numfaces
							for v = 1 to num_verts do
							(
							 vert = tmesh.verts[v]
							 convert = vert.pos.x as Integer
							 
							 vert.pos.x = convert as Integer
							 
							 convert = vert.pos.y as Integer
							 
							 vert.pos.y = convert as Integer
							 
							 convert = vert.pos.z as Integer
							 
							 vert.pos.z = convert as Integer
							
							)
							n = n + 1
						)
						x = x + 1
					)
					i = i + 1
				)
				
				messagebox "Finished"
					
				
				
			)-- end Undo
		
		
		
		
		
		
		)
		
		
		
	
		on addSection pressed do
		(
			Print "Adding Section - " + section_count as string
			
			
			
			
			searchstring = "groupcount"
			sourceobj = getNodeByName searchstring exact:true
			
			nextsection = undefined
			nextsection = box length:25 width:25 height:25 
			nextsection.name = "Section "+section_count as string	
			nextsection.pos=[section_count*50 , -1000 , -1500]
			
			childsection = undefined
			childsection = box length:15 width:15 height:15
			childsection.name = "Section "+section_count as string+" North"	
			childsection.pos=[section_count*50 , -1050 , -1500]
			childsection.parent = nextsection
			
			childsection = undefined
			childsection = box length:15 width:15 height:15
			childsection.name = "Section "+section_count as string+" East"	
			childsection.pos=[section_count*50 , -1100 , -1500]
			childsection.parent = nextsection
			
			childsection = undefined
			childsection = box length:15 width:15 height:15
			childsection.name = "Section "+section_count as string+" South"	
			childsection.pos=[section_count*50 , -1150 , -1500]
			childsection.parent = nextsection
			
			childsection = undefined
			childsection = box length:15 width:15 height:15
			childsection.name = "Section "+section_count as string+" West"	
			childsection.pos=[section_count*50 , -1200 , -1500]
			childsection.parent = nextsection
			
			childsection = undefined
			childsection = box length:15 width:15 height:15
			childsection.name = "Section "+section_count as string+" Collision"	
			childsection.pos=[section_count*50 , -1250 , -1500]
			childsection.parent = nextsection
			
			section_count = section_count + 1
			
			
			searchstring = "section count"
			sourceobj = getNodeByName searchstring exact:true
			if (sourceobj == undefined) then
			(
				sourceobj = box length:25 width:25 height:25 
				sourceobj.name = "section count"
			)
			
			sourceobj.pos=[section_count , -1500 , -1500]
			
			
			if (section_count > 1) then
				(
					sectioncount.Text = section_count as String + " Sections"
				)
				else
				(
					sectioncount.Text = section_count as String + " Section"
				)
			
		)

		on expbtn pressed do
		(
			
			output_name = getSavePath caption:"Export Directory"
			
			
			
			
			i = 1
			material_read = 0
			printstring = section_count as string
			
			
			
			
			while i <= section_count do
			(
				
				searchstring = "Section "+(i-1) as string
				sourceobj = getNodeByName searchstring exact:true
				
				
				sourcechild = undefined
				
				
				
				
				x = 1
				while x <= 4 do
				(
					sourcechild = sourceobj.children[x]
					
					select sourcechild.children
					
					
					
					local MaxSave = output_name + "\\" + (i-1) as string + (x-1) as string + ".obj"
					local plug = findItem exporterPlugin.classes ObjExp

					exportFile MaxSave #noPrompt selectedOnly:true using: plug
					
					x=x+1
				)
				i=i+1
			)
			
			messagebox "Finished"
			
		)


		
		
		on colbtn pressed do
		(
			
			output_name = getSavePath caption:"Export Directory"
			
			
			
			
			i = 1
			material_read = 0
			printstring = section_count as string
			
			
			while i <= section_count do
			(
				
				searchstring = "Section "+(i-1) as string
				sourceobj = getNodeByName searchstring exact:true
				
				
				sourcechild = undefined
				
				sourcechild = sourceobj.children[5]
				
				select sourcechild.children
				
				
				
				local MaxSave = output_name + "\\" + (i-1) as string + ".obj"
				local plug = findItem exporterPlugin.classes ObjExp

				exportFile MaxSave #noPrompt selectedOnly:true using: plug
				
				i = i + 1
				
			)
			
			messagebox "Finished"
			
		)
	
		-- End of rollout
	)		
CreateDialog MK64Vimport

