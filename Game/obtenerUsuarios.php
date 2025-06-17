<?php
include "dbConecction.php";

$sql = "SELECT id_usuario AS id, username FROM usuarios WHERE rol != 'admin'";
$result = $pdo->query($sql);
$usuarios = $result->fetchAll(PDO::FETCH_ASSOC);

header('Content-Type: application/json');
echo json_encode($usuarios);
?>