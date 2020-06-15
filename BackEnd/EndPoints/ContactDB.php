<?php
    
    require_once("../DB/DB.php");
    
    //Salva in un array i parametri contenuti nell'URL
    $queries = array();
	parse_str($_SERVER['QUERY_STRING'], $queries);
	
	$DB = new DB();
	$DB->CreateConnection();
	$res = $DB->ExecuteWithResponse($queries['query']);
	$JSONres = json_encode($res);
    print_r($JSONres);
?>