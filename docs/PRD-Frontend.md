# PRD - Loja Pedidos Web

## 1. Visão do projeto

O Loja Pedidos Web será um frontend simples para acompanhar e gerenciar os pedidos da API já desenvolvida. A ideia é permitir que uma pessoa avaliadora use os principais fluxos sem depender diretamente do Swagger.

O projeto será feito em Blazor e fará parte da mesma solução. A interface deve ser limpa, responsiva e fácil de entender, valorizando as regras de negócio que já existem no backend.

A identidade visual será inspirada nas cores utilizadas no site da Protech Solutions, sem copiar seu layout ou utilizar a marca como se o sistema fosse um produto oficial da empresa.

## 2. Objetivo

Criar uma interface que permita:

- visualizar os pedidos cadastrados;
- filtrar pedidos por CPF e status;
- navegar entre as páginas da listagem;
- consultar os detalhes de um pedido;
- criar um pedido com comprador e produtos;
- alterar a quantidade dos itens enquanto o pedido estiver iniciado;
- processar, enviar ou cancelar um pedido conforme as regras da API;
- excluir um pedido após confirmação;
- apresentar mensagens claras de sucesso, validação e erro.

O frontend não deve reproduzir regras de negócio que pertencem ao backend. Ele pode esconder ou desabilitar ações impossíveis para melhorar a experiência, mas a API continua sendo responsável por validar cada operação.

## 3. Público e cenário de uso

O sistema será usado principalmente por uma pessoa responsável pelo acompanhamento dos pedidos da loja.

O fluxo esperado é simples:

1. a pessoa acessa a listagem;
2. localiza um pedido por CPF ou status;
3. abre os detalhes;
4. altera quantidades ou avança o status quando permitido;
5. acompanha a mensagem retornada pela API.

Também será possível criar um pedido completo em uma única tela, informando comprador e produtos, seguindo o mesmo contrato do `POST /api/pedidos`.

## 4. Escopo do MVP

### 4.1 Listagem de pedidos

Será a página inicial do sistema.

Deve apresentar:

- número do pedido de forma abreviada, com opção de copiar o ID completo;
- nome do comprador;
- status em destaque;
- data de criação;
- valor total;
- quantidade de itens;
- ação para abrir os detalhes.

Filtros disponíveis:

- CPF do comprador;
- status do pedido;
- tamanho da página;
- botão para limpar os filtros.

A listagem deve usar a paginação retornada pela API e informar quando nenhum pedido for encontrado.

### 4.2 Criação de pedido

A tela deve permitir cadastrar todo o pedido de uma vez.

Dados do comprador:

- nome;
- CPF.

Dados de cada item:

- nome do produto;
- preço;
- quantidade.

Comportamentos esperados:

- começar com um item;
- permitir adicionar e remover itens;
- formatar CPF e moeda para facilitar o preenchimento;
- validar campos obrigatórios antes do envio;
- exibir um resumo do valor total estimado;
- impedir o envio duplo enquanto a requisição estiver em andamento;
- após o `201 Created`, mostrar a mensagem da API e abrir o pedido criado.

Se o CPF já estiver cadastrado, a API reutilizará o comprador e a mensagem retornada deve ser apresentada sem tratar isso como erro.

### 4.3 Detalhes do pedido

A página deve reunir as informações do pedido:

- identificador;
- comprador;
- status;
- datas;
- valor total;
- produtos, quantidades, preços unitários e subtotais.

Ações por status:

| Status atual | Ações disponíveis |
|---|---|
| Iniciado | Alterar quantidades, processar, cancelar e excluir |
| Processado | Enviar, cancelar e excluir |
| Enviado | Consultar e excluir |
| Cancelado | Consultar e excluir |

A alteração de quantidade deve usar o `ItemId` retornado pela API. O botão de salvar só deve ser habilitado quando existir alguma mudança válida.

A exclusão deve abrir uma confirmação explicando que ela remove o pedido definitivamente. Cancelar continua sendo uma mudança de status e não deve ser apresentado como exclusão.

## 5. Fora do escopo inicial

Para manter a entrega pequena e coerente com o backend, não fazem parte do primeiro momento:

- login ou autenticação com JWT;
- cadastro separado de compradores;
- cadastro ou catálogo separado de produtos;
- carrinho de compras;
- pagamento;
- controle de estoque;
- gráficos e indicadores gerenciais;
- edição do nome do comprador ou dos dados do produto;
- atualização em tempo real;
- tema escuro;
- publicação do frontend.

Esses itens só devem ser avaliados depois que o fluxo principal estiver funcionando e testado.

## 6. Estrutura de navegação

```text
/
└── redireciona para /pedidos

/pedidos
├── filtros e paginação
├── acesso aos detalhes
└── botão "Novo pedido"

/pedidos/novo
└── formulário de criação

/pedidos/{id}
├── dados do pedido
├── alteração de quantidades
├── atualização de status
└── exclusão
```

O menu principal terá apenas:

- Pedidos;
- Novo pedido.

Não há necessidade de sidebar no MVP. Um cabeçalho simples deixa mais espaço para a listagem e funciona melhor em telas menores.

## 7. Direção visual

### 7.1 Referência

A paleta foi baseada nas cores predominantes observadas no site da Protech Solutions em agosto de 2026. Ela será usada como inspiração visual, não como reprodução da identidade oficial.

### 7.2 Paleta inicial

| Uso | Cor | Aplicação |
|---|---|---|
| Azul principal | `#0F3560` | cabeçalho, títulos, botões principais e links |
| Laranja de destaque | `#DF542A` | ação principal, foco, indicadores e pequenos destaques |
| Azul de apoio | `#526F8E` | textos secundários e elementos menos importantes |
| Fundo claro | `#F5F7FA` | fundo geral das páginas |
| Branco | `#FFFFFF` | cards, formulários e tabelas |
| Texto principal | `#1F2937` | textos e valores |
| Borda | `#D8E0E8` | divisões, campos e tabelas |
| Sucesso | `#237A57` | confirmação de operações concluídas |
| Erro | `#B42318` | validações e falhas da API |

O laranja deve ser usado com moderação. Ele funciona melhor para chamar atenção para uma ação ou estado importante do que como cor de grandes áreas.

### 7.3 Tipografia e componentes

A referência visual usa Montserrat. Para evitar dependência obrigatória de fonte externa, o projeto poderá usar:

```css
font-family: "Montserrat", "Segoe UI", Arial, sans-serif;
```

Caso Montserrat seja adicionada, deve ser carregada de forma simples e documentada.

Estilo esperado:

- cabeçalho azul escuro;
- conteúdo com largura máxima e bom espaço lateral;
- cards brancos com borda discreta;
- cantos levemente arredondados, sem excesso;
- botões com texto direto: “Criar pedido”, “Salvar quantidades”, “Processar”;
- ícones apenas quando ajudarem a entender a ação;
- status representados por texto e cor, nunca somente pela cor;
- carregamento com skeleton simples ou indicador visível;
- mensagens próximas da ação que as originou.

## 8. Componentes principais

Os componentes devem surgir conforme forem usados. A estrutura inicial prevista é:

- `MainLayout`: cabeçalho e área principal;
- `PedidoStatusBadge`: apresentação consistente dos status;
- `PedidosTable`: tabela responsiva da listagem;
- `PedidosFilters`: filtros por CPF e status;
- `Pagination`: navegação das páginas;
- `PedidoForm`: formulário de criação;
- `PedidoItemsEditor`: inclusão de produtos e quantidades;
- `PedidoDetails`: resumo completo do pedido;
- `ConfirmDialog`: confirmação de exclusão e ações sensíveis;
- `AlertMessage`: mensagens de sucesso e erro;
- `LoadingState` e `EmptyState`: estados de carregamento e lista vazia.

Não é necessário criar todos antecipadamente. Um trecho só deve virar componente quando tiver responsabilidade própria ou for reutilizado.

## 9. Integração com a API

O frontend consumirá os endpoints existentes:

| Ação | Endpoint |
|---|---|
| Criar pedido | `POST /api/pedidos` |
| Listar pedidos | `GET /api/pedidos` |
| Consultar pedido | `GET /api/pedidos/{id}` |
| Alterar quantidades | `PUT /api/pedidos/{id}` |
| Alterar status | `PUT /api/pedidos/{id}/status` |
| Excluir pedido | `DELETE /api/pedidos/{id}` |

A URL da API não deve ficar espalhada pelo código. Ela será configurada em um único ponto, por exemplo:

```json
{
  "Api": {
    "BaseUrl": "http://localhost:5080"
  }
}
```

Para o ambiente do Compose, a URL poderá ser `http://localhost:8080`.

As classes usadas pelo frontend devem representar os contratos HTTP e não devem referenciar diretamente as entidades do Domain. Isso mantém o cliente independente do funcionamento interno da API.

## 10. Tratamento de respostas e erros

O frontend deve interpretar:

- `200 OK`: operação concluída ou consulta carregada;
- `201 Created`: pedido criado e redirecionamento para o detalhe;
- `400 Bad Request`: validação ou regra de negócio não permitida;
- `404 Not Found`: pedido inexistente ou removido;
- `500 Internal Server Error`: falha inesperada.

Quando a API retornar `ProblemDetails` ou `ValidationProblemDetails`, as mensagens devem ser apresentadas em português e próximas do formulário. Não deve ser exibido apenas “ocorreu um erro” quando a API já informou o motivo.

Falha de conexão deve ter uma mensagem própria, como:

> Não foi possível acessar a API. Verifique se o ambiente está em execução e tente novamente.

## 11. Responsividade e acessibilidade

O frontend deve funcionar em desktop e celular.

Requisitos mínimos:

- campos com `label` visível;
- navegação possível por teclado;
- foco bem destacado;
- contraste suficiente entre texto e fundo;
- mensagens que não dependam apenas de cor;
- botões desabilitados durante operações;
- tabela adaptada para cards ou rolagem controlada em telas pequenas;
- confirmação antes da exclusão;
- atributos acessíveis nos botões que usam somente ícones.

## 12. Requisitos técnicos

- Blazor com .NET 9, mantendo a mesma versão da API;
- projeto sugerido: `src/LojaPedidos.Web`;
- consumo HTTP com `HttpClient` e `System.Net.Http.Json`;
- configuração da URL da API por arquivo de ambiente;
- MudBlazor para os componentes visuais, com tema centralizado e CSS próprio apenas para identidade e ajustes específicos;
- nenhuma dependência adicionada sem uma necessidade real;
- frontend incluído na solução `LojaPedidos.sln`;
- componentes e páginas organizados por funcionalidade;
- tratamento central do resultado das chamadas HTTP quando isso reduzir repetição;
- nomes de classes e textos em português, seguindo o projeto atual.

A primeira escolha será Blazor WebAssembly porque o frontend será executado no navegador e consumirá a API REST diretamente. A política de CORS já existente na API será aproveitada e ajustada somente se a porta real do frontend for diferente das origens configuradas.

## 13. Critérios de aceite do MVP

O MVP estará pronto quando:

- a solução compilar com o novo projeto;
- a página inicial listar pedidos usando a API real;
- CPF e status filtrarem os resultados;
- a paginação respeitar os dados retornados pela API;
- for possível criar um pedido com um ou mais itens;
- a criação redirecionar para o pedido retornado pelo `201 Created`;
- o detalhe apresentar ItemId, produtos, preços, total e status;
- quantidades só puderem ser editadas no status Iniciado;
- as ações de status exibidas respeitarem o estado atual;
- uma transição rejeitada pela API mostrar a mensagem recebida;
- a exclusão pedir confirmação e voltar para a listagem;
- loading, vazio, erro e sucesso tiverem estados visíveis;
- o layout funcionar em desktop e celular;
- as cores e o estilo estiverem coerentes com a direção visual definida;
- os principais fluxos tiverem testes.

## 14. Testes previstos

### Testes de componentes

Prioridade inicial:

- badge apresenta todos os status corretamente;
- filtros montam os parâmetros esperados;
- formulário exige comprador e pelo menos um item;
- total estimado acompanha quantidade e preço;
- botões de status aparecem de acordo com o pedido;
- mensagens de `ProblemDetails` são exibidas.

### Testes de integração do frontend

Depois do fluxo principal:

- listagem carrega dados da API;
- criação envia o contrato correto e navega para o detalhe;
- alteração envia `ItemId` e nova quantidade;
- mudança de status atualiza a tela;
- exclusão confirmada remove o pedido da listagem.

Não será necessário testar detalhes internos do framework ou repetir no frontend todas as regras que já possuem testes no backend.

## 15. Etapas de desenvolvimento

A implementação será feita em partes pequenas:

1. criar o projeto Blazor e incluir na solução;
2. configurar tema, layout e URL da API;
3. criar os contratos e o cliente HTTP de pedidos;
4. implementar listagem, filtros e paginação;
5. implementar detalhes do pedido;
6. implementar criação do pedido;
7. implementar alteração de quantidades;
8. implementar ações de status;
9. implementar exclusão com confirmação;
10. revisar erros, carregamentos e responsividade;
11. adicionar os principais testes;
12. atualizar README e execução pelo Aspire.

Cada etapa deve gerar uma mudança pequena e fácil de revisar. A integração do frontend ao AppHost será feita quando o projeto já conseguir iniciar sozinho e consumir a API.

## 16. Decisões que devem continuar simples

- Não usar gerenciamento global de estado no MVP. O estado de cada página é suficiente.
- Não adicionar biblioteca de componentes antes de surgir uma necessidade concreta.
- Não criar uma camada genérica de repositório no frontend.
- Não copiar as entidades do backend; criar apenas contratos HTTP necessários.
- Não esconder mensagens relevantes retornadas pela API.
- Não duplicar as transições de status como regra definitiva no cliente.
- Não transformar a página inicial em dashboard com números inventados.

## 17. Resultado esperado

Ao final, a pessoa avaliadora deve conseguir abrir o sistema e entender o fluxo de pedidos sem precisar conhecer a estrutura interna da API. O frontend deve destacar o que já foi construído no backend, principalmente as validações, a paginação, os filtros e as transições de status.

A entrega visual deve parecer parte do mesmo projeto: organizada, profissional e direta, sem tentar competir com o escopo principal do desafio, que continua sendo o backend.

