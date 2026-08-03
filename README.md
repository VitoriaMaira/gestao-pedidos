<div align="center">

# 🛍️ Loja Pedidos

### Aplicação para gerenciar o ciclo de vida de pedidos de um e-commerce

Do recebimento ao envio, com regras de negócio, persistência, observabilidade e testes automatizados.

<p>
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dotnetcore/dotnetcore-original.svg" alt=".NET" width="58" height="58">&nbsp;&nbsp;&nbsp;
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/microsoftsqlserver/microsoftsqlserver-original.svg" alt="SQL Server" width="58" height="58">&nbsp;&nbsp;&nbsp;
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/docker/docker-original.svg" alt="Docker" width="68" height="58">&nbsp;&nbsp;&nbsp;
  <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/swagger/swagger-original.svg" alt="Swagger" width="58" height="58">
</p>

<p>
  <strong>.NET &nbsp;•&nbsp; SQL Server &nbsp;•&nbsp; Docker &nbsp;•&nbsp; Swagger</strong>
</p>

<p>
  <img alt=".NET 9" src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&amp;logo=dotnet&amp;logoColor=white">
  <img alt="ASP.NET Core" src="https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=for-the-badge&amp;logo=dotnet&amp;logoColor=white">
  <img alt="Entity Framework Core" src="https://img.shields.io/badge/EF_Core-9.0-512BD4?style=for-the-badge&amp;logo=dotnet&amp;logoColor=white">
</p>
<p>
  <img alt="SQL Server" src="https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&amp;logo=microsoftsqlserver&amp;logoColor=white">
  <img alt=".NET Aspire" src="https://img.shields.io/badge/.NET_Aspire-13.4-512BD4?style=flat-square&amp;logo=dotnet&amp;logoColor=white">
  <img alt="Docker" src="https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&amp;logo=docker&amp;logoColor=white">
  <img alt="xUnit" src="https://img.shields.io/badge/Tests-xUnit-5E2B97?style=flat-square">
  <img alt="Swagger" src="https://img.shields.io/badge/API-Swagger-85EA2D?style=flat-square&amp;logo=swagger&amp;logoColor=black">
</p>

</div>

---

## 📌 Sobre o projeto

A Loja Pedidos API resolve um fluxo completo de vendas: recebe o comprador e os produtos em uma única requisição, registra o pedido e controla cada etapa até o envio ou cancelamento.

O projeto vai além de um CRUD básico. A aplicação protege as transições de status, impede alterações indevidas, calcula o valor total, reutiliza compradores pelo CPF e mantém o preço praticado em cada item. Tudo isso é organizado em camadas para separar as regras de negócio do acesso ao banco e da API.

O ambiente também faz parte da entrega. A solução pode ser iniciada com .NET Aspire ou Docker Compose, possui banco persistente, migrations automáticas, documentação Swagger, telemetria, health checks e testes que exercitam tanto as regras isoladas quanto os fluxos HTTP reais.

### Destaques da entrega

| Área | O que foi desenvolvido |
|---|---|
| **Regras de negócio** | Ciclo completo do pedido, total calculado pelos itens e transições de status protegidas no domínio |
| **API REST** | Criação, consulta, alteração, mudança de status e exclusão, com códigos HTTP e respostas consistentes |
| **Frontend Web** | Interface em Blazor WebAssembly e MudBlazor para utilizar todo o fluxo da API |
| **Consultas** | Listagem paginada com filtros opcionais por CPF e status |
| **Persistência** | SQL Server, Entity Framework Core, migrations automáticas, repositórios e Unit of Work |
| **Ambiente local** | API e banco orquestrados pelo Aspire, com dashboard e volume persistente |
| **Observabilidade** | ServiceDefaults com health checks, logs, telemetria, resiliência e service discovery |
| **Distribuição** | Docker Compose preparado para consumir uma imagem pública da API |
| **Qualidade** | Validações, erros padronizados, Swagger documentado e testes unitários e de integração |

## ✅ Principais funcionalidades

- Criação de pedidos com comprador, produtos e itens em uma única operação.
- Reutilização do comprador quando o CPF já está cadastrado.
- Listagem paginada com filtros opcionais por CPF e status.
- Consulta de pedido por identificador.
- Alteração da quantidade dos itens de pedidos iniciados.
- Processamento, envio e cancelamento por atualização de status.
- Exclusão definitiva de pedidos.
- Validação de entrada com mensagens em português.
- Respostas de erro centralizadas com `ProblemDetails`.
- Documentação interativa com Swagger e exemplos de requisições.
- Health checks e telemetria fornecidos pelo ServiceDefaults.
- Testes unitários e de integração.

## 🖥️ Frontend

O projeto `LojaPedidos.Web` é uma aplicação Blazor WebAssembly com componentes MudBlazor. Ele consome os contratos HTTP da API e mantém o tratamento de erros centralizado no cliente de pedidos.

Pela interface é possível criar pedidos, consultar a listagem paginada, filtrar por CPF e status, abrir os detalhes, alterar quantidades, processar, enviar, cancelar e excluir. As ações disponíveis acompanham o status atual do pedido, enquanto a validação final e as regras de negócio permanecem na API.

A estrutura principal do projeto Web é:

```text
LojaPedidos.Web/
├── Clients/          # comunicação HTTP com a API
├── Components/       # layout e componentes compartilhados
├── Contracts/        # requests e responses usados pela interface
├── Pages/Pedidos/    # listagem, criação e detalhes
├── Theme/            # tema do MudBlazor
└── wwwroot/          # configurações e arquivos estáticos
```

## 🧰 Tecnologias utilizadas

- **.NET 9 e ASP.NET Core:** base da API REST.
- **Blazor WebAssembly e MudBlazor:** interface web responsiva para consumir a API.
- **Entity Framework Core 9:** mapeamento e persistência das entidades.
- **SQL Server 2022:** banco de dados relacional.
- **FluentValidation:** validação dos dados de entrada dos casos de uso.
- **.NET Aspire:** inicialização da API e do SQL Server, dashboard, logs e telemetria local.
- **Docker e Docker Compose:** execução da API publicada e do banco em containers.
- **Swagger / OpenAPI:** documentação interativa dos endpoints.
- **xUnit:** testes unitários e de integração.
- **Refit:** cliente HTTP usado nos testes de integração.

## 🏗️ Arquitetura

A solução separa as responsabilidades em projetos com dependências direcionadas para o domínio:

- **LojaPedidos.Api:** controllers, configuração HTTP, Swagger, CORS e filtro centralizado de erros.
- **LojaPedidos.Web:** páginas, componentes, contratos e cliente HTTP do frontend Blazor.
- **LojaPedidos.Application:** casos de uso, DTOs, validações e contratos usados pela aplicação.
- **LojaPedidos.Domain:** entidades, regras de negócio, value objects, exceções e contratos dos repositórios.
- **LojaPedidos.Infrastructure:** DbContext, configurações do Entity Framework, migrations, repositórios e Unit of Work.
- **LojaPedidos.AppHost:** orquestra a API e o SQL Server durante o desenvolvimento com Aspire.
- **LojaPedidos.ServiceDefaults:** configura health checks, service discovery, resiliência e OpenTelemetry.
- **LojaPedidos.UnitTests:** valida regras do domínio e entradas da aplicação.
- **LojaPedidos.IntegrationTests:** exercita a API em execução por HTTP com Refit.
- **deploy:** reúne o Compose e o exemplo das variáveis de ambiente.

```mermaid
flowchart LR
    Web[LojaPedidos.Web] --> API[LojaPedidos.Api]
    API --> Application[LojaPedidos.Application]
    Application --> Domain[LojaPedidos.Domain]
    Infrastructure[LojaPedidos.Infrastructure] --> Application
    Infrastructure --> Domain
    Infrastructure --> EF[Entity Framework Core]
    EF --> Database[(SQL Server)]
    AppHost[LojaPedidos.AppHost] -. orquestra .-> API
    AppHost -. orquestra .-> Web
    AppHost -. orquestra .-> Database
```

## 📋 Regras de negócio

- **Iniciado:** pode ter as quantidades alteradas, ser processado ou cancelado.
- **Processado:** não pode mais ser alterado, mas pode ser enviado ou cancelado.
- **Enviado:** pedido enviado ao comprador; é um estado final.
- **Cancelado:** pedido cancelado; é um estado final.

Também são protegidas regras como comprador obrigatório, CPF válido, pelo menos um item, produto com nome e preço válidos, quantidade maior que zero e proibição do mesmo produto mais de uma vez no pedido.

## 🚀 Como executar

Execute os comandos a partir da raiz do repositório.

### Pré-requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/), com Docker Compose

### Opção 1: .NET Aspire

Com o Docker Desktop em execução, inicie o AppHost:

```powershell
dotnet run --project src/LojaPedidos.AppHost
```

O Aspire cria o SQL Server com volume persistente, inicia a API e o frontend e apresenta no dashboard os recursos, logs, health checks e dados de telemetria. O endereço do dashboard é exibido no terminal. Use o link **Loja Pedidos Web** ou acesse `http://localhost:5056`. O Swagger da API fica disponível pelo link do recurso `api`.

### Opção 2: Docker Compose

O Compose utiliza a imagem pública:
API - `ghcr.io/vitoriamaira/loja-pedidos-api:latest` 
WEB - `ghcr.io/vitoriamaira/loja-pedidos-web:latest`


Já existe o arquivo local de configuração:

```
 deploy\.env
```

Altere `MSSQL_SA_PASSWORD` em `deploy/.env`. A senha precisa atender aos requisitos do SQL Server. Também é possível mudar `SQLSERVER_PORT` e `API_PORT`; os valores de exemplo são `1433` e `8080`. O arquivo `deploy/.env` contém informação sensível, está ignorado pelo Git e não deve ser enviado ao repositório.

Suba o ambiente em segundo plano:

```powershell
docker compose --env-file deploy\.env -f deploy\compose.yaml up -d
```

Acompanhe os logs:

```powershell
docker compose --env-file deploy\.env -f deploy\compose.yaml logs -f
```

Encerre os containers:

```powershell
docker compose --env-file deploy\.env -f deploy\compose.yaml down
```

O volume `sqlserver-data` mantém os dados entre reinicializações do ambiente.

O Compose atual inicia a API e o SQL Server. Para usar o frontend com esse ambiente, mantenha os containers ativos e execute em outro terminal:

```powershell
dotnet run --project src/LojaPedidos.Web --launch-profile compose
```

Nesse perfil, o frontend acessa a API em `http://localhost:8080` e fica disponível em `http://localhost:5056`.

## 🗄️ Banco de dados e migrations

As migrations ficam em `src/LojaPedidos.Infrastructure/Migrations`. Em ambiente de desenvolvimento, a API executa `MigrateAsync` na inicialização, portanto as migrations pendentes são aplicadas automaticamente tanto pelo Aspire quanto pelo Compose.

O repositório possui uma ferramenta local do Entity Framework. Para restaurá-la e criar uma nova migration:

```powershell
dotnet tool restore
dotnet ef migrations add NomeDaMigration --project src/LojaPedidos.Infrastructure --startup-project src/LojaPedidos.Api --output-dir Migrations
```

Depois, basta iniciar novamente a aplicação em ambiente de desenvolvimento para aplicar a migration.

## 📖 Documentação da API

- Com Docker Compose: `http://localhost:8080/swagger`
- Com Aspire: use o link **Swagger** exibido para o recurso `api` no dashboard.

Principais rotas:

- `POST /api/pedidos`
- `GET /api/pedidos/{id}`
- `GET /api/pedidos?pagina=1&tamanhoPagina=10&cpf=...&status=...`
- `PUT /api/pedidos/{id}`
- `PUT /api/pedidos/{id}/status`
- `DELETE /api/pedidos/{id}`

O Swagger inclui descrições e exemplos reais para os corpos das requisições. Em desenvolvimento, os endpoints `/health` e `/alive` informam a disponibilidade da API.

## 💡 Exemplos de uso

### Criar um pedido

`POST /api/pedidos`

```json
{
  "comprador": {
    "nome": "João da Silva",
    "cpf": "52998224725"
  },
  "itens": [
    {
      "produto": {
        "nome": "Teclado mecânico",
        "preco": 150.00
      },
      "quantidade": 2
    }
  ]
}
```

A resposta é `201 Created`, contém os identificadores gerados e informa no cabeçalho `Location` a rota de consulta do novo pedido.

### Processar um pedido

`PUT /api/pedidos/{id}/status`

```json
{
  "status": "Processado"
}
```

## 🧪 Testes

O projeto `LojaPedidos.UnitTests` cobre entidades, transições de status e validações dos requests. O projeto `LojaPedidos.IntegrationTests` verifica os principais fluxos HTTP, incluindo criação, consulta, paginação, filtros, erros, CORS e regras de mudança de status.

Testes unitários:

```powershell
dotnet test tests\LojaPedidos.UnitTests\LojaPedidos.UnitTests.csproj
```

Os testes de integração consomem uma API real. Inicie o ambiente com Aspire e execute:

```powershell
dotnet test tests\LojaPedidos.IntegrationTests\LojaPedidos.IntegrationTests.csproj
```

Por padrão, eles acessam `http://localhost:5080`. Para testar o Compose, informe a URL antes da execução:

```powershell
$env:LOJA_PEDIDOS_API_URL = "http://localhost:8080"
dotnet test tests\LojaPedidos.IntegrationTests\LojaPedidos.IntegrationTests.csproj
```

Com a API ativa e a URL configurada quando necessário, toda a solução pode ser testada com:

```powershell
dotnet test LojaPedidos.sln
```

## ✨ Decisões e boas práticas

A organização em camadas mantém as regras do pedido independentes da API e do Entity Framework. Os controllers recebem e devolvem DTOs, enquanto os casos de uso coordenam validações, repositórios e a unidade de trabalho. Essa separação deixa o fluxo mais fácil de entender, testar e evoluir sem adicionar abstrações fora do escopo.

- Injeção de dependência para casos de uso, repositórios e serviços.
- Entidades responsáveis por proteger as regras e transições de status.
- DTOs específicos para entrada e saída, sem expor diretamente as entidades.
- FluentValidation para validar requests antes da execução dos casos de uso.
- Repositórios e Unit of Work para isolar e confirmar a persistência em uma única operação.
- Mapeamentos do EF Core separados com `IEntityTypeConfiguration<T>` e carregados pelo assembly.
- GUIDs ordenáveis, CPF com restrição de unicidade e valores monetários configurados no banco.
- Paginação e filtros opcionais para evitar consultas de listagem sem limite.
- Tratamento centralizado com respostas `ProblemDetails` e mensagens compreensíveis.
- Configuração sensível por variável de ambiente e arquivo `.env` fora do Git.
- SQL Server persistido em volumes no Aspire e no Compose.
- Dockerfile em múltiplas etapas, execução com usuário não root e imagem disponível no GHCR.
- Health checks, telemetria pelo ServiceDefaults e testes automatizados em dois níveis.
- Swagger com descrições e exemplos compatíveis com os contratos reais da API.

## 📁 Estrutura do projeto

```text
src/
├── LojaPedidos.Api
├── LojaPedidos.Web
├── LojaPedidos.Application
├── LojaPedidos.Domain
├── LojaPedidos.Infrastructure
├── LojaPedidos.AppHost
└── LojaPedidos.ServiceDefaults

tests/
├── LojaPedidos.UnitTests
└── LojaPedidos.IntegrationTests

deploy/
├── .env.example
└── compose.yaml
```

## Considerações finais

O projeto reúne em uma única entrega o fluxo de negócio, a API, a persistência, os testes e a infraestrutura necessária para execução. A proposta foi manter o código simples de acompanhar, mas completo nos pontos que sustentam uma API real: regras consistentes, dados persistidos, ambiente reproduzível, documentação e validação automatizada.



