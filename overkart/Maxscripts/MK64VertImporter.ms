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
							
							
						while not eof in_file do 
						(
							
							v1x = readLine in_file as float
							v1z = readLine in_file as float
							v1y = readLine in_file as float
							
							mybox = box length:5 width:5 height:5 
							mybox.pos=[v1x,v1y,v1z]
						)
						
						close in_file		
								
		
								
					)
				)							
			
						
						
				
			)	
		
			
			
			
			
			
			-- End of rollout
			
			
			
					
		)		
CreateDialog DuckDodget

