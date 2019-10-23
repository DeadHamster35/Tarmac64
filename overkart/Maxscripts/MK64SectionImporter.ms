vertlist = ""
vertfile = #()
object_array = #()
garbage = "string"
vertname = "string"


	rollout MK64Section "S-Importer" width:80 height:68
		(	
			GroupBox grp3 "S-Importer" pos:[3,0] width:75 height:62	
			label Lbl "<3 Hamp" enabled:false pos:[10,53]
			button impBtn "Import" pos:[10,20] width:60 height:25
			
			
			
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
										
										
										vertbox.pos = [n*5000,m*5000,0]																				
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
									execute vertlist
									object_array[i] = vertbox	
									if (i > 1) then
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

