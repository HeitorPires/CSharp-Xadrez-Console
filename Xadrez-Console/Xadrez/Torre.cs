  using tabuleiro;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(cor, tab)
        {
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

            //Norte
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            while(Tab.PosicalValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.Linha -= 1;
            }

            //Sul
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            while (Tab.PosicalValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.Linha += 1;
            }

            //Leste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            while (Tab.PosicalValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.Coluna += 1;
            }

            //Oeste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            while (Tab.PosicalValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.Coluna -= 1;
            }


            return mat;
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
