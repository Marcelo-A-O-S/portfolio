export const blogs = [
    {
        "id": "post_01",
        "title": "Dashboard de Monitoramento Industrial",
        "slug": "dashboard-monitoramento-industrial",
        "description": "Aplicação web que monitora dados em tempo real de sensores industriais via WebSocket.",
        "keywords": "dashboard, industrial, sensores, websocket",
        "content": "Este projeto tem como objetivo monitorar sensores de temperatura, vibração e umidade em tempo real. Utiliza React para a interface, NestJS no backend e WebSocket para comunicação contínua.",
        "tools": [
            { "id": "tool_01", "name": "React", "description": "Framework para interfaces web." },
            { "id": "tool_02", "name": "NestJS", "description": "Framework backend modular com suporte a WebSockets." },
            { "id": "tool_03", "name": "PostgreSQL" }
        ],
        "imageUrl": "https://cdn.exemplo.com/imgs/dashboard.png",
        "types": [
            { "id": "type_01", "name": "PROJECT" },
            { "id": "type_02", "name": "ACADEMIC" }
        ],
        "isPublished": true,
        "createdAt": "2025-07-14T12:00:00Z",
        "updatedAt": "2025-07-14T12:00:00Z",
        "authorId": "user_01",
        "author": {
            "id": "user_01",
            "name": "Marcelo Augusto",
            "email": "marcelo@example.com",
            "image": "https://cdn.exemplo.com/imgs/marcelo.png"
        },
        "Categories": [
            {
                "id": "cat_01",
                "name": "Sistemas Embarcados"
            }
        ],
        "likes": [],
        "comments": [],
        "links": [
            {
                "id": "link_01",
                "linkTypeId": "linktype_01",
                "content": "https://github.com/Marcelo-A-O-S/monitoramento-industrial",
                "linkType": { "id": "linktype_01", "name": "GITHUB" }
            }
        ]
    },
    {
        "id": "post_02",
        "title": "Como Montar um Medidor de Umidade com Arduino",
        "slug": "tutorial-arduino-umidade",
        "description": "Tutorial para iniciantes com foco em automação agrícola com sensores de umidade.",
        "keywords": "arduino, umidade, sensor, automação",
        "content": "Neste tutorial, vamos montar um sistema básico de detecção de umidade usando o Arduino Uno e sensor YL-69. Ideal para quem está iniciando na área de automação.",
        "tools": [
          { "id": "tool_04", "name": "Arduino" },
          { "id": "tool_05", "name": "Sensor YL-69", "description": "Sensor de umidade para solo." }
        ],
        "imageUrl": "https://cdn.exemplo.com/imgs/arduino-umidade.png",
        "types": [
          { "id": "type_03", "name": "TUTORIAL" }
        ],
        "isPublished": true,
        "createdAt": "2025-07-13T10:00:00Z",
        "updatedAt": "2025-07-13T10:00:00Z",
        "authorId": "user_01",
        "author": {
          "id": "user_01",
          "name": "Marcelo Augusto",
          "email": "marcelo@example.com",
          "image": "https://cdn.exemplo.com/imgs/marcelo.png"
        },
        "Categories": [
          {
            "id": "cat_02",
            "name": "Automação Residencial"
          }
        ],
        "likes": [],
        "comments": [],
        "links": [
          {
            "id": "link_02",
            "linkTypeId": "linktype_01",
            "content": "https://github.com/Marcelo-A-O-S/tutorial-arduino",
            "linkType": { "id": "linktype_01", "name": "GITHUB" }
          },
          {
            "id": "link_03",
            "linkTypeId": "linktype_02",
            "content": "https://youtube.com/watch?v=exemplo123",
            "linkType": { "id": "linktype_02", "name": "YOUTUBE" }
          }
        ]
      },{
        "id": "post_03",
        "title": "Reflexões sobre Engenharia de Controle em Projetos Reais",
        "slug": "engenharia-controle-projetos-reais",
        "description": "Uma análise pessoal sobre os desafios da aplicação prática de engenharia de controle.",
        "keywords": "engenharia, controle, projeto, automação",
        "content": "Neste artigo compartilho minha experiência pessoal desenvolvendo sistemas de controle com ESP32 em ambientes industriais, refletindo sobre teoria vs. prática.",
        "tools": [
          { "id": "tool_06", "name": "ESP32" },
          { "id": "tool_07", "name": "PID Control" }
        ],
        "imageUrl": "https://cdn.exemplo.com/imgs/controle-tecnico.png",
        "types": [
          { "id": "type_04", "name": "PERSONAL" },
          { "id": "type_01", "name": "PROJECT" }
        ],
        "isPublished": false,
        "createdAt": "2025-07-10T15:00:00Z",
        "updatedAt": "2025-07-11T09:30:00Z",
        "authorId": "user_01",
        "author": {
          "id": "user_01",
          "name": "Marcelo Augusto",
          "email": "marcelo@example.com",
          "image": "https://cdn.exemplo.com/imgs/marcelo.png"
        },
        "Categories": [
          {
            "id": "cat_03",
            "name": "Controle e Automação"
          }
        ],
        "likes": [],
        "comments": [],
        "links": [
          {
            "id": "link_04",
            "linkTypeId": "linktype_03",
            "content": "https://linkedin.com/in/marcelo-eng",
            "linkType": { "id": "linktype_03", "name": "LINKEDIN" }
          }
        ]
      }
]