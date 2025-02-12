document.addEventListener('DOMContentLoaded', () => {
    fetch('https://localhost:7190/api/articles')
        .then(response => response.json())
        .then(articles => {
            const articleList = document.getElementById('article-list');
            articles.forEach(article => {
                const listItem = document.createElement('li');
                const link = document.createElement('a');
                link.href = article.url;
                link.textContent = `${article.title} (${new Date(article.publishedDate).toLocaleDateString()})`;
                link.target = '_blank';
                listItem.appendChild(link);
                articleList.appendChild(listItem);
            });
        })
        .catch(error => console.error('Error fetching articles:', error));
});
