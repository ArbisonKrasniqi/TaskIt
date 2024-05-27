/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./public/**/*/html', './src/**/*.{js,jsx}'],
  theme: {
    extend: {
      colors: {
        costumColor: '#515151',
        customColor: '#2C4167',
      }
    },
  },
  content: [
    "./src/**/*.{js,ts,jsx,tsx}",
    "./public/index.html",
  ],
  theme: {},
  plugins: [],
};