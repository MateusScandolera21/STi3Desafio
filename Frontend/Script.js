document.addEventListener('DOMContentLoaded', () => {
    generateNewIdentifier(); // Gera um identificador ao carregar a página
    setCurrentDate(); // Define a data atual ao carregar a página

    // Adiciona o evento de clique ao botão de recarregar
    document.getElementById('reloadButton').addEventListener('click', () => {
        location.reload(); // Recarrega a página
    });
});

// Função para gerar um novo identificador
function generateNewIdentifier() {
    const identificadorField = document.getElementById('identificador');
    identificadorField.value = self.crypto.randomUUID();
}

// Função para definir a data atual
function setCurrentDate() {
    const dataVendaField = document.getElementById('dataVenda');
    const today = new Date().toISOString().split('T')[0];
    dataVendaField.value = today;
}

// Função para resetar o formulário
function resetForm() {
    document.getElementById('orderForm').reset(); // Reseta todos os campos do formulário
    generateNewIdentifier(); // Gera um novo identificador
    setCurrentDate(); // Define a data atual novamente
}

// Função para rolar a página para o topo
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth' // Rolagem suave
    });
}

document.getElementById('orderForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    const formData = {
        identificador: document.getElementById('identificador').value,
        dataVenda: document.getElementById('dataVenda').value,
        cliente: {
            clienteId: self.crypto.randomUUID(),
            nome: document.getElementById('clienteNome').value,
            cpf: document.getElementById('clienteCPF').value,
            categoria: document.getElementById('clienteCategoria').value,
        },
        itens: [
            {
                produtoId: parseInt(document.getElementById('produtoId').value),
                descricao: document.getElementById('produtoDescricao').value,
                quantidade: parseInt(document.getElementById('produtoQuantidade').value),
                precoUnitario: parseFloat(document.getElementById('produtoPrecoUnitario').value),
            },
        ],
    };

    const resultDiv = document.getElementById('result');
    const loadingBar = document.getElementById('loadingBar');
    const loadingBarProgress = document.querySelector('.loading-bar-progress');
    const statusLabel = document.getElementById('statusLabel');

    // Exibe a barra de carregamento
    loadingBar.style.display = 'block';
    loadingBarProgress.style.width = '0';

    // Simula o progresso da barra de carregamento
    let progress = 0;
    const interval = setInterval(() => {
        progress += 10;
        loadingBarProgress.style.width = `${progress}%`;
        if (progress >= 100) clearInterval(interval);
    }, 300);

    try {
        const response = await fetch('http://localhost:5292/api/vendas', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        resultDiv.textContent = `Success: ${JSON.stringify(data, null, 2)}`;

        // Exibe o status "CONCLUÍDO"
        statusLabel.textContent = 'CONCLUÍDO';
        statusLabel.style.display = 'block';

        // Reseta o formulário após o sucesso
        resetForm();

        // Rola a página para o topo
        scrollToTop();
    } catch (error) {
        resultDiv.textContent = `Error: ${error.message}`;
    } finally {
        // Oculta a barra de carregamento
        loadingBar.style.display = 'none';
    }
});