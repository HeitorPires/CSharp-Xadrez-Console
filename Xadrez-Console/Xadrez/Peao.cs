using tabuleiro;

namespace xadrez
{
    internal class Peao : Peca
    {
        private PartidaDeXadrez partida;

        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(cor, tab)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool ExisteInimigo(Posicao pos)
        {
            Peca p = Tab.GetPeca(pos);
            return p != null && p.Cor != Cor;
        }

        private bool Livre(Posicao pos)
        {
            return Tab.GetPeca(pos) == null;
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.GetPeca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];
            Posicao pos = new(0, 0);

            //Peao Branco
            if (Cor == Cor.Branca)
            {
                //Norte uma casa
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //Norte duas casas
                pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos) && QteMovimentos == 0)
                    mat[pos.Linha, pos.Coluna] = true;

                //Noroeste
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //Nordeste
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                // #Jogadaespecial EnPassant
                if (Posicao.Linha == 3)
                {
                    Posicao esquerda = new(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.GetPeca(esquerda) == partida.VulneravelEnPassant)
                        mat[esquerda.Linha - 1, esquerda.Coluna] = true;

                    Posicao direita = new(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.GetPeca(direita) == partida.VulneravelEnPassant)
                        mat[direita.Linha - 1, direita.Coluna] = true;
                }
            }
            //Peao Preto
            else
            {
                //Sul uma casa
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //Sul duas casas
                pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos) && QteMovimentos == 0)
                    mat[pos.Linha, pos.Coluna] = true;

                //Sudoeste
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                //Sudeste
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    mat[pos.Linha, pos.Coluna] = true;

                // #Jogadaespecial EnPassant
                if (Posicao.Linha == 4)
                {
                    Posicao esquerda = new(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.GetPeca(esquerda) == partida.VulneravelEnPassant)
                        mat[esquerda.Linha + 1, esquerda.Coluna] = true;

                    Posicao direita = new(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.GetPeca(direita) == partida.VulneravelEnPassant)
                        mat[direita.Linha + 1, direita.Coluna] = true;
                }
            }

            return mat;
        }
    }
}
