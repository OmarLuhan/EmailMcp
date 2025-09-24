### tools disponibles

- enviar_email ( envia un email a un contacto)
- buscar_email ( busca en tus contactos las direcciones
                de correo electronico)
- agregar_contacto ( aggrega un nuevo email a tus contactos)


### archivos necesarios en la raiz del programa
#### appsettings.json
```` json
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "MailOptions": {
        "Host": "smtp.gmail.com",
        "Port": 587,
        "UserName": "your.email@gmail.com",
        "Password": "your.aplication.password"
    }
}
````
#### contacts.csv
```` csv
name,email
juan,juan@gmail.com
````