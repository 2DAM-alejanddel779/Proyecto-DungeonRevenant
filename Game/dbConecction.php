<?php

try {
    $pdo = new PDO('mysql:host=localhost;dbname=game', 'unity', '1234');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('SET NAMES "utf8"');
}
catch (PDOException $e){
    echo "Error conectando con la base de datos " . $e->getMessage();
    exit();
}

?>