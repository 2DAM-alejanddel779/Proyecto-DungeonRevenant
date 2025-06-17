<?php
include "dbConecction.php";

$sql = "SELECT u.username, MAX(r.puntuacion) AS puntuacion
        FROM rankings r
        JOIN usuarios u ON u.id_usuario = r.usuario_id
        GROUP BY u.id_usuario
        ORDER BY puntuacion DESC
        LIMIT 10";

$result = $pdo->query($sql);

$ranking = [];

foreach ($result as $row) {
    $ranking[] = [
        'usuario' => $row['username'],
        'puntuacion' => $row['puntuacion']
    ];
}

header('Content-Type: application/json');
echo json_encode($ranking);
exit();
?>