document.addEventListener('DOMContentLoaded', () => {
    const identificadorField = document.getElementById('identificador');
    const dataVendaField = document.getElementById('dataVenda');

    // Generate a UUID for Identificador
    identificadorField.value = self.crypto.randomUUID();

    // Set Data Venda to today's date
    const today = new Date().toISOString().split('T')[0];
    dataVendaField.value = today;
});

// Handle form submission
document.getElementById('orderForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    // Gather form data
    const formData = {
        identificador: document.getElementById('identificador').value,
        dataVenda: document.getElementById('dataVenda').value,
        cliente: {
            clienteId: self.crypto.randomUUID(), // Generate a UUID for clienteId
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

    try {
        // Call your local API
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
    } catch (error) {
        resultDiv.textContent = `Error: ${error.message}`;
    }
});