import './App.css';
import Rotas from './Routes';
import { Analytics } from '@vercel/analytics/react'

function App() {
  return (
    <div className="App">
       <Rotas/>
       <Analytics/>
    </div>
  );
}

export default App;
