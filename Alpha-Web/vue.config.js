module.exports = {
  devServer: {
    proxy: {
      '/api': {
        target: 'https://localhost:44397',
        changeOrigin: true,
        secure: false, // Se estiver usando HTTPS com certificado autoassinado
        logLevel: 'debug',
      },
    },
  },
};
