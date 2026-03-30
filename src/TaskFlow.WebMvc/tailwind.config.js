/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class', 
  content: [
    "./Views/**/*.cshtml",
    "./wwwroot/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        primary: '#98CA3F',
        'primary-container': '#b2df5b',
        'on-primary-container': '#1a2b00',
        background: '#060e1f',
        'on-surface': '#f8fafc',
        'on-surface-variant': '#94a3b8',
        'surface-container-lowest': '#020617',
        'surface-container-low': '#0f172a',
        'surface-container': '#1e293b',
        'surface-container-high': '#334155',
        'surface-container-highest': '#475569',
        'outline-variant': '#334155',
        error: '#ef4444',
        tertiary: '#F0E04A',
        'tertiary-dim': '#d1c028'
      },
      fontFamily: {
        headline: ['Manrope', 'sans-serif'],
        sans: ['Inter', 'sans-serif']
      }
    },
  },
  plugins: [],
}