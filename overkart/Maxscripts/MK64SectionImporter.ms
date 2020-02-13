vertlist = ""
vertfile = #()
object_array = #()
garbage = "string"
vertname = "string"
position = #()


	rollout MK64Section "S-Importer" width:175 height:68
		(	
			GroupBox grp3 "S-Importer" pos:[5,0] width:165 height:62
			label Lbl "<3 Hamp" enabled:false pos:[10,53]
			button impBtn "Import" pos:[10,20] width:60 height:25
			checkbox split "Split Sections" pos:[75,25] checked:true
			
			
			on duckDodget open do
			(
			
				ClearListener()
				
					
			)
		
			
		
			on impBtn pressed do
			(
				clearlistener()
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
							if (vertlist == "ENDSECTION") then
							(
								
									
									object_array = undefined
									object_array = #()
									i=1
									
									n=n+1
									if (n == 4) then
									(
										n = 0
										m = m + 1
									)
									
									Print "section end"
									gc()
								
								
								garbage = readLine in_file as string
								garbage = readLine in_file as string
								
							)
							else
							(
							
								if (vertlist == "NEWOBJECT") or (vertlist == "ENDOBJECT") then
								(
									if i > 1 then
									(
										
										if split.Checked == true do
										(
											vertbox.pos = [n*10000,m*10000,0]	
										)
										vertbox.name = vertname
										
										vertbox = undefined
										
										object_array = undefined
										object_array = #()
										i=1
									)
									garbage = readLine in_file as string
									vertname = readLine in_file as string
									Print "object end"
									
									
								
								)
								else
								(
									facelist = FilterString vertlist ";"
									print "facelist"
									print facelist[1]
									print facelist[2]
									print facelist[3]
									print "---"
									
									for face in facelist do
									(
										vert = FilterString face ","
										print "verts"
										print vert[1]
										print vert[2]
										print vert[3]
										print "---"
										join position vert 
										
									)
									
									print "i value"
									print i as string
									print "---"
									
									
									vertbox = mesh vertices:#([position[1] as float,position[2] as float,position[3] as float],[position[4] as float,position[5] as float,position[6] as float],[position[7] as float,position[8] as float,position[9] as float]) faces:#([1,2,3]) MaterialIDS:#(1) 		
									object_array[i] = vertbox	
									position.Count = 0
									
									
									if (i > 1) do
									(
										meshop.attach object_array[i] object_array[i-1]									
									)
										
									i=i+1
								)
							)
						)
						
						
						
							
		
								
					)
				)							
			
				close in_file
				gc()		
				
			)	
		
			
			
			
					
		)		
CreateDialog MK64Section

