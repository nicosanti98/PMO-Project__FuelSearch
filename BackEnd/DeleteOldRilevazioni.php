<?php

    require_once('DB/DB.php');

    class DeleteOldRilevazioni
    {
        public function Delete()
        {
            $DB = new DB();
            $DB->CreateConnection();
            $query = "DELETE FROM StoricoRilevazioni WHERE dtComu < '".date(("Y-m-d"), strtotime("-2 week"))."'";
            $DB->Execute($query);
            $DB->DestroyConnection();
        }
    }

?>