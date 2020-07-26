file=""
file_name=""

maxver = maxVersion()

----------------------------------------------------------------------------------------------------
-- MK64 Path Importer
-- <3 hamp
----------------------------------------------------------------------------------------------------


Global x = 0
Global y = 0
Global z = 0
Global flag = 0
Global vertfile = #()

pathcount = 0
markercount = 0

vert_count = 0
vert_array = #()
face_array = #()



vertbox = mesh
parentpath = mesh
lastflag = 0
nextflag = 0
boxr=0
boxg=0
boxb=0

Global line_count = 0





rollout MK64Vimport "P-Importer" width:80 height:85
	(	
		GroupBox grp3 "P-Importer" pos:[3,0] width:75 height:80
		label Lbl "<3 Hamp" enabled:false pos:[10,53]
		button impBtn "Import" pos:[10,20] width:60 height:25
		button expBtn "Export" pos:[10,50] width:60 height:25
		
		
		
		on MK64Vimport open do
		(
		
			ClearListener()
			temp_buffer = fopen (sysInfo.tempdir + "\\float_conv_buffer.txt") "wb+"
			
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
				
					i=0
					n=0
					groupcount = 0
					groupcount = readLine in_file as float
					
					while not eof in_file do 
					(
						
						readstring = readLine in_file
						
						if readstring == "NEW GROUP" then
						(
							grandparentpath = undefined
							grandparentpath = box length:25 width:25 height:25 
							readstring = readLine in_file							
							grandparentpath.name = readstring	
							xval = readLine in_file as float
							grandparentpath.pos=[xval , 0 , 0]
							n=n+1
						)
						else
						(
							if readstring == "NEW PATH" then
							(
								parentpath = undefined
								parentpath = box length:25 width:25 height:25 
								readstring = readLine in_file							
								parentpath.name = readstring	
								
								
								parentpath.parent=grandparentpath
								n=n+1
							)
							else
							(
								v1x = readstring as float
								v1y = readLine in_file as float
								v1z = readLine in_file as float
								flag = readLine in_file as integer
								
								if (flag != lastflag) then
								(
									lastflag = flag
									boxr = random 0 255
									boxg = random 0 255
									boxg = random 0 255
								
								
								)
								
								mybox = box length:25 width:25 height:25 
								mybox.pos=[v1x*-1,v1y,v1z]
								mybox.name = "marker"+i as string+" - "+flag as string
								mybox.parent = parentpath
								mybox.wirecolor = color boxr boxg boxb
								i=i+1
							)
						)
					)
					
					
					mybox = box length:.035 width:.035 height:.035 
					mybox.pos=[groupcount,0,0]
					mybox.name = "groupcount" as string
					
					close in_file	
				)
			)			
		)

		on expbtn pressed do
		(
			
			output_name = getSaveFileName caption:"Exporting Verts" \
			filename: "" \
			types:"TXT (*.txt)|*.txt|All Files (*.*)|*.*|"
			
			i = 0
			
			if output_name == undefined then 
			(
			
			)
			else
			(
				txt = createfile output_name
				
				
				searchstring = "groupcount"
				sourceobj = getNodeByName searchstring exact:true
				groupcount = sourceobj.pos.x
				
				total_instance_count=0
				
				n = 0
				
				while n < groupcount do
				(
					x = 1
					searchstring = "GROUP "+n as string
					gpaobj = getNodeByName searchstring exact:true
					format "NEW GROUP" to:txt
					format "\n" to:txt
					pathcount = gpaobj.children.count
					while x <= pathcount do
					(
					
						searchstring = "PATH "+x as string
						paobj = gpaobj.children[x]
						format "NEW PATH" to:txt
						format "\n" to:txt
						markercount = paobj.children.count
						
						i=1
						while i <= markercount do
						(
					
							
							markerobj = paobj.children[i]
							
							printstring = (markerobj.pos.x*-1) as string +","+(markerobj.pos.y) as string +","+(markerobj.pos.z) as string
							format printstring to:txt
							format "\n" to:txt
							i= i+1
						)
						
						x=x+1
					)
					n=n+1
				)
			
				
				
				
				
				close txt
				
				
			)
		)
	
		-- End of rollout
	)		
CreateDialog MK64Vimport

