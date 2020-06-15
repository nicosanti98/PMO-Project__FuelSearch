<?php

    //Definizione costanti per accesso a DB
    define ('USERNAME', '***');
    define('HOST', '***');
    define('PASSWORD', '***');
    define('DATASET', '***');
    
    class DB
    {
        //Variabile contenente la connessione al DB
        private $conn; 
        
        //Metodo pubblico che crea la connessione al DB
        public function CreateConnection()
        {
            //Creo l'oggetto connessione al db passadogli host, 
            //username e password e dataset
            $conn = new mysqli(HOST, USERNAME, PASSWORD, DATASET);
            $this->conn = $conn;
            if(!$conn)
            {
                echo "Errore di connessione al DB";
            }
            
        }
        
        //Metodo che esegue la query passata per argomento
        public function Execute($query)
        {
            if($this->conn == null)
            {
                echo "\nConnessione al DB non creata";
            }
            else
            {
                if($this->conn->query($query) === TRUE)
                {
                    
                }
                else
                {
                    echo "\nErrore query!"; 
                    echo $this->conn->error;
                }
                    
                
            
            }
        }
        //Metodo per ottenere anche la stringa
        public function ExecuteWithResponse($query)
        {
            if($this->conn == null)
            {
                echo "\nConnessione al DB non creata";
                return null; 
            }
            else
            {
                $result = $this->conn->query($query); 
				$outp = $result->fetch_all(MYSQLI_ASSOC);
                return $outp;
            }
        }
        //Metodo per chiudere la connessione al DB
        public function DestroyConnection()
        {
            $this->conn->close();
        }
    }
    
?>
