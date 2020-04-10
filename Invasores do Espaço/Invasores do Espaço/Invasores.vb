﻿Public Class Invasores
    Dim JogadorDireita As Boolean
    Dim JogadorEsquerda As Boolean
    Dim VelocidadeJogador As Integer = VelocidadeJogadorAlteravel
    Dim VelocidadeTiro As Integer = VelociadeTiroAlteravel
    Dim VelocidadeInvasor As Integer = VelocidadeInvasoresAlteravel
    Dim IvasorCair As Integer = 50
    Dim NumDeInvasores As Integer = VelocidadeNumeroInvasoresAlteravel ' Escrever quantos invasores pertendo que sejam aparecidos na tela
    Dim InvasorDireita(NumDeInvasores) As Boolean
    Dim Invasores(NumDeInvasores) As PictureBox
    Dim X As Integer 'X =Counter
    Dim TiroCair As Integer
    Dim Pausa As Boolean = False
    Dim NInvasoresMortos As Integer = 0
    Dim TempoPartida As Integer = 0

    Private Sub TimerPrincipal_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerPrincipal.Tick
        'Timer 1 temporalizador
        TemporalizadorDeJogo()
        MoverJogador()
        MoverTiro()
        MoverInvasor()
        VerTiro()
        GameOver()
        RotinaPontuacao()
    End Sub

    Private Sub Invasores_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Tecla é precionada para baixo
        'Ao clicar nas teclas (A ou D) permitir mover
        'D - Direita
        If e.KeyValue = Keys.D Then
            JogadorDireita = True 'Permite a ida para Direita
            JogadorEsquerda = False 'Bloqueia a ida para a Esquerda
        End If

        'A - Esquerda
        If e.KeyValue = Keys.A Then
            JogadorEsquerda = True 'Permite a ida para Esquerda
            JogadorDireita = False 'Bloqueia a ida para a Direita
        End If

        'Espaço - Tiro
        'Se o espaço for permido e o tiro visiver então
        If e.KeyValue = Keys.Space And PictTiro.Visible = False Then
            PictTiro.Visible = True 'Torna o tiro visivel
            'A picterbox do tiro vai ter com o topo da nave do jogardor
            PictTiro.Top = NaveJogador.Top
            'A picterbox do tiro começa a subir no eixo do Y de onde foi lançada
            PictTiro.Left = NaveJogador.Left + (NaveJogador.Width / 2) - (PictTiro.Width / 2)
        End If
    End Sub

    Private Sub MoverJogador()
        'Se estiver permitido mover então anda mais ou mneos um consuante a sua posição
        'Não permite que o jogador passe a tela
        If (JogadorDireita = True) And (NaveJogador.Left + NaveJogador.Width < Me.ClientRectangle.Width) Then
            NaveJogador.Left += VelocidadeJogador
        End If
        If (JogadorEsquerda = True) And (NaveJogador.Left > Me.ClientRectangle.Left) Then
            NaveJogador.Left -= VelocidadeJogador
        End If
    End Sub

    Private Sub Invasores_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        'Tecla é levantada
        'Bloqueia a permiçao de mover
        'D - Direita
        If e.KeyValue = Keys.D Then
            'Bloqueia o movimento para a esquerda e para a direita
            JogadorDireita = False
            JogadorEsquerda = False
        End If

        'A - Esquerda
        If e.KeyValue = Keys.A Then
            'Bloqueia o movimento para a esquerda e para a direita
            JogadorEsquerda = False
            JogadorDireita = False
        End If
    End Sub

    Private Sub Invasores_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Loading Inicial
        OpcoesInvasores()
        OpcoesGeral()
    End Sub
    Private Sub OpcoesGeral()
        'Opções de tudo
        VelocidadeJogador = VelocidadeJogadorAlteravel 'Velocidade jogador
        VelocidadeTiro = VelociadeTiroAlteravel 'Velocidade  Tiro

        'Para todos os invasores faz isto
        For Me.X = 1 To NumDeInvasores
            InvasorDireita(X) = True 'Permitir o invasor ir para a direita
            Invasores(X).Left = (-50 * X) - (X * 5) 'Invasores começarem todos na esquerda e fora da tela
            Invasores(X).Top = 0 'Manda os invasores comçar no topo da tela
            Invasores(X).Visible = True 'Torna os invasores visiveis
        Next

        PictTiro.Visible = False 'Tiro invisivel

        IvasorCair = 50 'Indicação quanto o invasor deve sair na tela

        'Começar de novo
        NInvasoresMortos = 0
        VelocidadeInvasor = VelocidadeInvasoresAlteravel
        TiroCair = 0
        JogadorDireita = False
        JogadorEsquerda = False
        TimerPrincipal.Enabled = True
        Label2.Text = NInvasoresMortos
    End Sub

    Private Sub MoverTiro()

        If PictTiro.Visible = True Then 'Se o tiro estiver visivel
            PictTiro.Top -= VelocidadeTiro 'O tiro começa a andar para cima
        End If

        'Se da parte de cima da pictorbox até ao "cu" da mesma e for maior que a barra do topo da tela então
        If PictTiro.Top + PictTiro.Height < Me.ClientRectangle.Top Then
            PictTiro.Visible = False 'o tiro fica invisivel
        End If
    End Sub

    Private Sub MoverInvasor()
        'Cria um ciclo para todos os invesores que foi ordenado existirem
        For Me.X = 1 To NumDeInvasores
            If InvasorDireita(X) = True Then 'Se o invasor estiver a ir para a direita ou seja a Direita esiver Ativa
                Invasores(X).Left += VelocidadeInvasor 'Manda os invasores delocarem para a Direita

            Else 'Se o invasor estiver a ir para a Esquerda ou seja a Dirita esiver ATIVADA
                Invasores(X).Left -= VelocidadeInvasor 'Manda os invasores delocarem para a Esquerda
            End If

            'Se de um lado ao outro do invasor, da direita para a esquerda, o tamanho do invasor estiver a tocar na direita da tela, e o invasor estiver direcionado para a direita, então
            If Invasores(X).Left + Invasores(X).Width > Me.ClientRectangle.Width And InvasorDireita(X) = True Then
                InvasorDireita(X) = False 'Andar para a dirita fica DESATIVADO
                Invasores(X).Top += IvasorCair 'Manda o invasor cair um degrau em relação ao topo
            End If

            'Se a Esquerda do invasor estiver  tocar na esqueda da tela, e o invasor estiver direcionado para a Esquerda, então
            If Invasores(X).Left < Me.ClientRectangle.Left And InvasorDireita(X) = False Then
                InvasorDireita(X) = True 'Andar para a dirita fica ATIVO
                Invasores(X).Top += IvasorCair 'Manda o invasor cair um degrau em relação ao topo
            End If
        Next
    End Sub

    Private Sub GameOver()
        'Derrota
        For Me.X = 1 To NumDeInvasores
            'Se algum invasor atingir o bico da nave, então o jogo acaba
            If Invasores(X).Top + Invasores(X).Height >= NaveJogador.Top And Invasores(X).Visible = True Then
                TimerPrincipal.Enabled = False 'Parar o timer
                Me.X = NumDeInvasores 'Dizer para parar de fazer o ciclo
                MsgBox("Game Over - A terra foi Invadida") 'Informar que a terra foi invadida e que perdeu
                NovoJogo() 'Perguntar se quer começar de novo
            End If
        Next

        'Vitória
        If TiroCair = NumDeInvasores Then 'Se a pontuação foi igual ao número de invasores "Vitória"
            TimerPrincipal.Enabled = False 'Timer parar
            MsgBox("A Terra está Salva") 'Informar que a terra foi salva e que venceu
            NovoJogo() 'Perguntar se quer começar de novo
        End If
    End Sub

    Private Sub VerTiro()
        For Me.X = 1 To NumDeInvasores
            If (PictTiro.Top + PictTiro.Height >= Invasores(X).Top) And (PictTiro.Top <= Invasores(X).Top + Invasores(X).Height) And (PictTiro.Left + PictTiro.Width >= Invasores(X).Left) And (PictTiro.Left <= Invasores(X).Left + Invasores(X).Width) And (PictTiro.Visible = True) And (Invasores(X).Visible = True) Then
                Invasores(X).Visible = False
                PictTiro.Visible = False
                TiroCair += 1
                NInvasoresMortos = TiroCair
                Label2.Text = NInvasoresMortos 'Mostrar os pontos
            End If
        Next
    End Sub

    Private Sub OpcoesInvasores()
        'Comçar todos os invasores do lado da tela
        For Me.X = 1 To NumDeInvasores
            'Cria uma PictureBox para cada um dos invasores que foi ordenado aparecer
            Invasores(X) = New PictureBox
            Invasores(X).Image = My.Resources.invader 'Vai buscar aos recursos a imagem do invasor
            Invasores(X).Width = 50 'Diz quanto mede a PictureBox
            Invasores(X).Height = 50 'Diz quanto mede a PictureBox
            Invasores(X).BackColor = Color.Transparent 'A PictureBox de cor irá ser transparente
            Invasores(X).SizeMode = PictureBoxSizeMode.StretchImage 'A imagem irá se ajustar á PictureBox
            Controls.Add(Invasores(X)) 'Para fazer os comandos a cima ditos resultar
        Next
    End Sub

    Private Sub Invasores_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        'Pausa
        If e.KeyChar = "p" Or e.KeyChar = "P" Then 'Ao clicar na letra "p"
            If Pausa = True Then 'Se pausa não estiver ativo
                TimerPrincipal.Enabled = True 'Parar o timer que manda para tudo, daí ser uma pausa
                Label1.Visible = False 'Label que diz "Pausa fica visivel"
                Pausa = False 'Pausa fica ativo
            Else
                TimerPrincipal.Enabled = False 'O timer volta a funcionar
                Label1.Visible = True 'A laber que diz "Pausa vai desaparecer"
                Pausa = True 'Pausa fica desativo e pronto para ser ativo quando voltar a clicar no "P"
            End If
        End If

        'Sair
        If e.KeyChar = "x" Or e.KeyChar = "X" Then 'Au clicar na letra "X"
            Me.Close() 'Fecha o formulário
        End If
    End Sub

    Private Sub NovoJogo()
        Dim jogarNovamente = MsgBox("Queres jogar de novo?", MsgBoxStyle.YesNo) 'Cria variabel que vai receber uma mensagagembox de sim ou não
        If jogarNovamente = MsgBoxResult.Yes Then 'se o resultado for sim
            OpcoesGeral() 'Começa de novo
        Else 'Se receber não
            Me.Close() 'Fecha o formulário
        End If
    End Sub

    Sub RotinaPontuacao()
        'Pontuação

    End Sub

    Sub TemporalizadorDeJogo()
        'contabiliza o tempo de jogo jogado
        TempoPartida += 1 'Aumenta o tempo jogado
        Label3.Text = TempoPartida 'Mostra o tempo jogado
    End Sub
End Class