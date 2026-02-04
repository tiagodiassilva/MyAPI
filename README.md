# Visão Geral - Funcionalidades da API Financeira

Este documento detalha as alterações feitas na aplicação `MyApp` para incluir recursos de cotação de moedas e ações em tempo real, utilizando o framework .NET 8.

## Alterações Verificadas

### 1. Integração da API Financeira
- **Controller**: Criado `FinanceController.cs` com três endpoints:
    - `GET /api/finance/currency`: Retorna a taxa de câmbio atual de USD para BRL.
    - `GET /api/finance/stock`: Retorna a cotação da bolsa B3 IBOVESPA.
    - `GET /api/finance/summary`: Retorna ambos os dados de moeda e ações.
- **Models**: Criado `FinanceModels.cs` para deserializar respostas de APIs externas (AwesomeAPI e HG Brasil).
- **Program.cs**: Registrados `Controllers` e `HttpClient`.

### 2. Atualização do Dockerfile
- **Build Multi-estágio**: Atualizado o `Dockerfile` para usar um processo de build em múltiplos estágios para imagens menores e mais seguras.
- **Exposição de Porta**: Exposta a porta `8080`.
- **Otimização**: Otimizado o cache de camadas copiando o `.csproj` e restaurando antes de copiar o código-fonte.

## Resultados da Verificação

### Verificação de Build
- `dotnet build` executado com sucesso com 0 erros e 0 avisos.
- `docker build -t myapp .` executado com sucesso.

## Como Executar

### Desenvolvimento Local
```bash
dotnet run
```
Acesse os endpoints em:
- `http://localhost:5000/api/finance/summary`

### Docker
```bash
docker run -p 8080:8080 myapp
```
Acesse os endpoints em:
- `http://localhost:8080/api/finance/summary`

## Referência dos Endpoints da API

| Método | Endpoint | Descrição |
|--------|----------|-------------|
| GET | `/api/finance/currency` | Busca a taxa de câmbio USD-BRL |
| GET | `/api/finance/stock` | Busca detalhes do IBOVESPA |
| GET | `/api/finance/summary` | Visualização agregada de Moeda e Ações |
