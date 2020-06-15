<?php
    
    require_once('DownloadItems.php');
    require('Parsers/ParseAnagrafica.php');
    require('Parsers/ParseRilevazioni.php');
    require_once('DB/DB.php');
    require_once('DeleteOldRilevazioni.php');


        //Costanti contenenti gli URL contenenti i dati
    define('URLANAGRAFICA', 'https://www.mise.gov.it/images/exportCSV/anagrafica_impianti_attivi.csv');
    define('URLRILEVAZIONI', 'https://www.mise.gov.it/images/exportCSV/prezzo_alle_8.csv');

    //Scarico la stringa contenente la lista delle rilevazioni
    $Download = new DownloadItems(URLRILEVAZIONI);
    $Response = $Download->GetDownloadedString();
    //Creo una lista di oggetti rilevazioni parsando la stringa scaricata
    $Parser = new ParseRilevazioni();
    $Parser->BuildObject($Parser->SplitString($Response));
    $ListRilevazioni = $Parser->TakeList();
    $DB = new DB();
    //Cancello tutto il contenuto della tebella rilevazioni e ne setto il 
    //prossimo indice a 1
    $DB->CreateConnection();
    $query = "DELETE FROM Rilevazioni"; 
    $DB->Execute($query);
    $query = "ALTER TABLE Rilevazioni AUTO_INCREMENT=1";
    $DB->Execute($query);
    
    //Per ogni rilevazione inserisco i sui dati nel DB
    $i; 
    for($i = 0; $i<count($ListRilevazioni); $i++)
    {
        $query =
        "INSERT INTO Rilevazioni
        (idImpianto, descCarburante, prezzo, isSelf, dtComu)
        VALUES
        (".$ListRilevazioni[$i]->idImpianto.",'".$ListRilevazioni[$i]->descCarburante."','".$ListRilevazioni[$i]->prezzo."','".
        $ListRilevazioni[$i]->isSelf."','".$ListRilevazioni[$i]->dtComu."')";
        $DB->Execute($query);
        
        //inserisce le rilevazioni anche nella tabella "storico rilevazioni"
        $query =
        "INSERT INTO StoricoRilevazioni
        (idImpianto, descCarburante, prezzo, isSelf, dtComu)
        VALUES
        (".$ListRilevazioni[$i]->idImpianto.",'".$ListRilevazioni[$i]->descCarburante."','".$ListRilevazioni[$i]->prezzo."','".
        $ListRilevazioni[$i]->isSelf."','".$ListRilevazioni[$i]->dtComu."')";
        $DB->Execute($query);
    }
    $DB->DestroyConnection();

    //Ripeto il procedimento per     
    $Download = new DownloadItems(URLANAGRAFICA);
    $Response = $Download->GetDownloadedString();
    
    $Parser = new ParseAnagrafica();
    $Parser->BuildObject($Parser->SplitString($Response));
    $ListAnagrafica = $Parser->TakeList();
    $DB = new DB();
    $DB->CreateConnection();
    $query = "DELETE FROM AnagraficaImpianto";
    $DB->Execute($query);
    $query = "ALTER TABLE AnagraficaImpianto AUTO_INCREMENT=1";
    $DB->Execute($query);
    
    $i;
    for($i = 0; $i<count($ListAnagrafica); $i++)
    {
        
    $query =
    "INSERT INTO AnagraficaImpianto
    (idImpianto, Gestore, Bandiera, TipoImpianto, NomeImpianto, Indirizzo, Comune, Provincia, Latitudine, Longitudine)
    VALUES
    (".$ListAnagrafica[$i]->idImpianto.",'".$ListAnagrafica[$i]->Gestore."','".
    $ListAnagrafica[$i]->Bandiera."','".$ListAnagrafica[$i]->TipoImpianto."','".
    $ListAnagrafica[$i]->NomeImpianto."','".$ListAnagrafica[$i]->Indirizzo."','".$ListAnagrafica[$i]->Comune."','".$ListAnagrafica[$i]->Provincia."','".
    $ListAnagrafica[$i]->Latitudine."','".$ListAnagrafica[$i]->Longitudine."')";
    $DB->Execute($query);
    }
    
    $DB->DestroyConnection();
    
    $Delete = new DeleteOldRilevazioni();
    $Delete->Delete();
    
    
    

?>