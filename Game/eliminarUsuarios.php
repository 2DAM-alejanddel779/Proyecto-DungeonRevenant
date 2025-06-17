<?php
include "dbConecction.php";

$id = $_POST['id'];
$sql = "DELETE FROM usuarios WHERE id_usuario = ?";
$stmt = $pdo->prepare($sql);
$resultado = $stmt->execute([$id]);

header('Content-Type: application/json');
echo json_encode(["exito" => $resultado]);
?>