**Desafio do Balta.io - API de Localização (IBGE) em .NET 7.0**

Desafio do Balta.io - API de Localização (IBGE) em .NET 7.0
Este é um projeto em .NET 7.0 criado como parte do Desafio do Balta.io. O objetivo principal deste projeto é desenvolver uma API com recursos de autenticação e autorização, um sistema CRUD para Localização (IBGE) e a capacidade de filtrar dados por cidade, estado e código IBGE. Além disso, o projeto inclui a funcionalidade de importação de dados a partir de um arquivo Excel (.XLS).

A importação com arquivo .XLS retorna um objeto contendo informações de quantos registros foram criados, ignorados e uma lista com as localizações que tiveram erro, assim como os campos inválidos e a linha do arquivo que está o erro, neste formato:

{
  "elapsed": 00:00:xx,
  "createdCount": 0,
  "ignoredCount": 0,
  "failedCount": 0,
  "failedLocalities": [
    {
      "row": 0,
      "isValid": true,
      "locality": {
        "id": 0,
        "ibgeCode": 0,
        "city": "string",
        "uf": "string"
      },
      "invalidFields": [
        "string"
      ]
    }
  ]
}

**Documentação swagger: http://ibge-challenge.ddns.net**
