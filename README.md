# Arcade Runner 2D 🏎️

Protótipo inicial de um jogo 2D arcade procedural estilo *infinite traffic runner*, desenvolvido na **Unity 2022.3 LTS** utilizando **C#** e o pipeline gráfico **Universal 2D (URP)**.

Projeto estruturado como material prático de apoio para curso de extensão universitária e desenvolvimento de jogos 2D.

---

## 📌 Visão Geral do Protótipo

O projeto implementa a dinâmica clássica de desvio de tráfego e obstáculos com rolagem vertical contínua, aplicando técnicas de ilusão 2.5D por meio de *Y-sorting* e sombras projetadas.

### 🎮 Mecânicas do Jogo
- **Controle do Jogador:** Movimentação horizontal contínua e travamento de bordas da pista (`Mathf.Clamp`).
- **Pista Infinita (Reciclagem de Blocos):** Sistema de *pooling* e reposicionamento contínuo de segmentos de estrada no eixo vertical.
- **Tráfego Procedural:** Geração dinâmica de obstáculos (cones, poças de óleo) e veículos em faixas sorteadas.
- **Efeitos Visuais 2.5D:** Sombras projetadas e ordenação dinâmica de profundidade de sprites.

---

## 🛠️ Tecnologias Utilizadas

- **Engine:** Unity 2022.3 LTS (Template Universal 2D / URP)
- **Linguagem:** C# (.NET Standard)
- **Controle de Versão:** Git / GitHub
- **Assets Gráficos:** [Mini Pixel Pack 2](https://grafxkid.itch.io/mini-pixel-pack-2) por GrafxKid (Pixel Art Arcade)

---

## 📋 Roadmap do Desenvolvimento

- [x] Configuração inicial da engine e repositório
- [x] Importação e calibração de sprites (*Pixel Perfect*, PPU, Point Filter)
- [ ] Implementação do controlador do jogador (`PlayerRunner.cs`)
- [ ] Sistema de rolagem infinita da pista (`ScrollingBackground.cs`)
- [ ] Gerenciador de spawn de obstáculos e tráfego
- [ ] Detecção de colisões e tela de Game Over
- [ ] Contador de pontuação por distância percorrida

---

## 🚀 Como Executar Localmente

1. Clone o repositório:
   ```bash
   git clone [https://github.com/GustavoKntsu/arcade-runner-2d.git](https://github.com/GustavoKntsu/arcade-runner-2d.git)
