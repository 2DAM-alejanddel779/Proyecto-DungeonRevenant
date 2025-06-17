<?php 
include "dbConecction.php";

// Obtener datos POST
$usuario = $_POST['usuario'];
$puntuacion = intval($_POST['puntuacion']);

// Obtener ID usuario
$sql = "SELECT id_usuario FROM usuarios WHERE username = '$usuario'";
$result = $pdo->query($sql);

if ($result->rowCount() === 0) {
    echo json_encode(['valido' => false, 'mensaje' => "Usuario no encontrado."]);
    exit();
}

$row = $result->fetch();
$id_usuario = $row['id_usuario'];

// Consulta para comprobar si tiene puntuacion el usuario
$sqlComprobacion = "SELECT puntuacion FROM rankings WHERE usuario_id = $id_usuario";
$resultado = $pdo->query($sqlComprobacion);

if ($resultado->rowCount() > 0) {

    // Si tiene puntuacion se compara
    $rowCheck = $resultado->fetch();
    $puntuacionExistente = $rowCheck['puntuacion'];

    // Comprobacion de si es mayor la puntuacion
    if ($puntuacion > $puntuacionExistente) {
        
        $sqlUpdate = "UPDATE rankings SET puntuacion = $puntuacion WHERE usuario_id = $id_usuario";
        $updateResultado = $pdo->query($sqlUpdate);

        if ($updateResultado) {
            echo json_encode(['valido' => true, 'mensaje' => "Puntuación actualizada."]);
        } else {
            echo json_encode(['valido' => false, 'mensaje' => "Error al actualizar la puntuación."]);
        }
    } else {
        // La nueva puntuación no es mayor
        echo json_encode(['valido' => true, 'mensaje' => "La puntuación no es mayor que la existente."]);
    }
} else {
    // Si no existe puntuacion insertamos
    $sqlInsert = "INSERT INTO rankings (puntuacion, puesto, usuario_id) VALUES ($puntuacion, 0, $id_usuario)";
    $insertResultado = $pdo->query($sqlInsert);

    if ($insertResultado) {
        echo json_encode(['valido' => true, 'mensaje' => "Puntuación guardada correctamente."]);
    } else {
        echo json_encode(['valido' => false, 'mensaje' => "Error al guardar la puntuación."]);
    }
}
?>
