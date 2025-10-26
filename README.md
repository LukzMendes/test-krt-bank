
# KRT Bank Test - Projeto de Teste

📌 Objetivo
Este projeto é um teste técnico com o objetivo de criar uma API para gerenciamento de contas bancárias, permitindo operações de CRUD sobre contas de clientes.


Cada conta possui:
- ID
- Numero da conta
- Saldo
- Status da conta (Ativa/Inativa)
- Dados do usuario titular como CPF e Nome. 

O projeto simula eventos para integração com áreas do banco .

📌 Tecnologias
- .NET 8
- Arquitetura: DDD, MVC, SOLID, Clean Code
- Minimal API + Swagger
- Testes: xUnit
- Cache: IMemoryCache para evitar consultas repetidas
- Banco: Repositórios em memória (Não foi possívle criar o banco e fila na aws. O projeto foi criado pensando no sql então contem os mappings para migrations, repositórios e um exemplo superficial de uma mensageria.)

📌 Estratégia de Cache
Para reduzir custos com consultas repetidas, foi implementado CachedBankAccountRepository usando IMemoryCache.
- Cada conta é armazenada no cache por 1 dia, com chave baseada em ID e data.
- Operações de Update e Remove atualizam ou removem o cache.

📌 Como Executar
1. Clone o repositório:
2. Abra no Visual Studio ou CLI.
3. Defina o projeto test-Krt-bank como inicialização.
4. Execute: irá abrir o swagger com todos os endpoints. 

📌 Endpoints Principais
Usuários
- POST /api/user → Criar usuário
  Body:
  {
    "name": "João Silva",
    "cpf": "12345678900"
  }
- GET /api/user/all → Listar usuários

Contas Bancárias
- POST /api/bank-account → Criar conta
  Body:
  {
    "userId": "guid-do-usuario",
    "accountNumber": "12345"
  }

**Antes de criar uma conta é necessário criar um usuário**

📌 Melhorias Futuras
- Integração real com banco de dados (SQL/AWS).
- Publicação de eventos em mensageria (AWS).
- Cache distribuído (Redis).
- Implementação completa do CRUD.

📌 Testes
- Framework: xUnit
- Cobertura: Casos de uso e validações de domínio.


att: Lucas Guilherme Mendes Caetano
