<?php
include "dbConecction.php";

// Obtener datos POST 
$usuario = $_POST['usuario'];
$email = $_POST['email'];
$password = hash("sha256", $_POST['pass']);

// Verificar usuario
$sql = "SELECT username FROM usuarios WHERE username = '$usuario'";
$result = $pdo->query($sql);

if ($result->rowCount() > 0) {
    header('Content-Type: application/json');
    echo json_encode(['valido' => false, 'mensaje' => "Error, el nombre de usuario ya existe."]);
    exit();
}

// Verificar email
$sql = "SELECT email FROM usuarios WHERE email = '$email'";
$result = $pdo->query($sql);

if ($result->rowCount() > 0) {
    header('Content-Type: application/json');
    echo json_encode(['valido' => false, 'mensaje' => "Error, el email ya está siendo utilizado."]);
    exit();
}

// Insertar usuario
$sql = "INSERT INTO usuarios (username, email, password, rol) VALUES ('$usuario', '$email', '$password', 'usuario')";
$insertResult = $pdo->query($sql);

if ($insertResult) {
    header('Content-Type: application/json');
    echo json_encode(['valido' => true, 'mensaje' => "Usuario creado correctamente."]);
} else {
    header('Content-Type: application/json');
    echo json_encode(['valido' => false, 'mensaje' => "Error al crear el usuario."]);
}


exit();
?>
