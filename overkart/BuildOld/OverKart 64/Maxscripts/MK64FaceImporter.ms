file=""
file_name=""

maxver = maxVersion()

----------------------------------------------------------------------------------------------------
-- HRS Importer
-- <3 hamp
----------------------------------------------------------------------------------------------------


Global x = 0
Global y = 0
Global z = 0
Global vertfile = #()
vert_count = 0
vert_array = #()
face_array = #()

object_array = #()

vertbox = mesh

Global line_count = 0





	rollout DuckDodget "V-Importer" width:80 height:68
		(	
			GroupBox grp3 "V-Importer" pos:[3,0] width:75 height:62	
			label Lbl "<3 Hamp" enabled:false pos:[10,53]
			button impBtn "Import" pos:[10,20] width:60 height:25
			
			
			
			on duckDodget open do
			(
			
				ClearListener()
				temp_buffer = fopen (sysInfo.tempdir + "\\float_conv_buffer.bin") "wb+"
					
			)
		
			
		
			on impBtn pressed do
			(
				Print "Startup Import"
				in_name = getOpenFileName \
					filename: "" \
					types: "TXT File (*.txt)|*.txt|All Files (*.*)|*.*"
				if in_name != undefined then
				(
			
					in_file = openfile in_name
					
					if in_file != undefined then
					(
						m = 0
						n = 0	
						i = 1
						while not eof in_file do 
						(
						
							
						
							vertlist = readLine in_file as string
							if vertlist == "ENDSECTION" then
							(
								
								i=1								
								object_array=#()
								
							)
							else
							(
								v1x = readLine in_file as float
								v1z = readLine in_file as float
								v1y = readLine in_file as float
								v1s = readLine in_file as float
								v1t = readLine in_file as float
								
								v2x = readLine in_file as float
								v2z = readLine in_file as float
								v2y = readLine in_file as float
								v2s = readLine in_file as float
								v2t = readLine in_file as float
								
								v3x = readLine in_file as float
								v3z = readLine in_file as float
								v3y = readLine in_file as float
								v3s = readLine in_file as float
								v3t = readLine in_file as float
								
								vertbox = mesh vertices:#([v1x,(0-v1y),v1z],[v2x,(0-v2y),v2z],[v3x,(0-v3y),v3z]) tverts:#([v1s,(v1t),0],[v2s,(v2t),0],[v3s,(v3t),0]) faces:#([1,2,3]) MaterialIDS:#(1) 
								mmesh = vertbox.mesh
								buildTVFaces mmesh
								for n = 1 to mmesh.numfaces do
								(
									setTVFace mmesh n (getFace mmesh n)
									setFaceSmoothGroup mmesh n 1
									
								)
								
								addModifier vertbox (Normalmodifier flip:true)
								collapseStack vertbox
								
								
								
								
								object_array[i] = vertbox	
								if i > 1 then
								(
									meshop.attach object_array[i] object_array[i-1]
									currentobject = object_array[i]
								)
								
								i=i+1
							)
						)
						
						close in_file		
						
						gc()
						clearUndoBuffer()	
		
								
					)
				)							
			
						
						
				
			)	
		
			
			
			
					
		)		
CreateDialog DuckDodget

