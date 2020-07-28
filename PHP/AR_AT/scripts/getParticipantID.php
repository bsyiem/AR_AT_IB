<?php
	/*
	if(($file = fopen("../files/last_pid.csv","r")) !== FALSE)
	{
		$data = fgetcsv($file,10,",");
		if($data !== FALSE)
		{
			echo($data[0]);
		}
	}	
	fclose($file);
	*/
	
	if(($file = fopen("../files/last_pid.csv","r")) !== FALSE)
	{
		//using an exclusive lock instead of a shared lock as I only want pid to be unique
		if(flock($file, LOCK_EX))
		{
			$data = fgetcsv($file,10,",");
			if($data !== FALSE)
			{
				echo($data[0]);
			}
			
			flock($file,LOCK_UN);
		}else{
			echo "Error: Please Reload Page";
		}
		fclose($file);
	}else{
		echo "Error: Please Reload Page";
	}	
?>
