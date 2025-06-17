<?php
include "dbConecction.php";

$usuario = $_POST['usuario'];
$password = hash("sha256", $_POST['pass']);

$sql = "SELECT username, rol FROM usuarios WHERE username = '$usuario' AND password = '$password'";
$result = $pdo-> query($sql);

if($result->rowCount() > 0){
    $user = $result->fetch();

    $esAdmin = 0;
    if ($user['rol'] == 'admin'){
        $esAdmin = 1;
    }

    header('Content-Type: application/json');
    echo json_encode(['valido' => true, 'mensaje' => "Usuario: $usuario", 'esAdmin' => $esAdmin]);
    exit();
} else {
     header('Content-Type: application/json');
    echo json_encode(['valido' => false, 'mensaje' => "Error, nombre de usuario o contraseña incorrectos."]);
    exit();
}

?>