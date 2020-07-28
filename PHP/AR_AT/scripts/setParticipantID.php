<?php
	
	$pId = $_POST["pId"];

	if($pId !== ""){
		if(($file = fopen("../files/last_pid.csv","w")) !== FALSE){
			if(flock($file,LOCK_EX)){
				fputcsv($file,array($pId));
			}else{
				echo "Error: Please Reload Page";
			}
			fclose($file);
		}
	}
	
?>