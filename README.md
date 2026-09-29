# AppWeb-Adrian

Sistema web em Blazor Server para consulta e cadastro de processos.

## Banco de dados

A aplicação usa MySQL com a conexão configurada em `appsettings.json`:

- **Servidor:** `localhost`
- **Porta:** `3360`
- **Banco:** `pds_app_web`
- **Usuário:** `root`
- **Senha:** `root`

> Em um ambiente real, altere a senha e não mantenha credenciais padrão.

### Criar o banco

Com o servidor MySQL em execução, rode o script:

```bash
mysql -h127.0.0.1 -P3360 -uroot -p < Configs/script_banco_pds_app_web.sql
```

O script cria o banco `pds_app_web`, a tabela `processos` e um registro inicial de exemplo. Ele pode ser executado novamente sem duplicar esse registro.

### Estrutura da tabela `processos`

| Coluna | Tipo | Descrição |
|---|---|---|
| `id_pro` | `INT` | Identificador automático |
| `numero_pro` | `VARCHAR(200)` | Número do processo |
| `data_pro` | `DATE` | Data do processo |
| `interessado_pro` | `VARCHAR(200)` | Interessado |
| `assunto_pro` | `VARCHAR(200)` | Assunto |
| `descricao_pro` | `TEXT` | Descrição detalhada |
| `situacao_pro` | `VARCHAR(50)` | Situação atual |

## Executar o projeto

```bash
dotnet restore
dotnet run
```

Acesse `/processos` para consultar os processos e `/processos/cadastro` para cadastrar um novo registro.
