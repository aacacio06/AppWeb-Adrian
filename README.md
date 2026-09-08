# AppWeb-Adrian

## Banco de dados

No MySQL Workbench, abra o arquivo `Configs/script_banco_app_web_bd.sql`, conecte-se ao servidor local e execute o script inteiro. Ele cria o banco `app_web_bd`, a tabela `processos` e um registro de exemplo. O script pode ser executado novamente sem duplicar esse registro.

A aplicação está configurada para conectar em `localhost:3360`, usuário `root`, senha `root`. Se a sua instalação do MySQL usa a porta padrão `3306`, altere a porta em `appsettings.json` antes de executar o projeto.

Depois de executar o script e iniciar a aplicação, acesse `/processo` ou `/processos`.
