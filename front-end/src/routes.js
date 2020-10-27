import React from 'react'
import { Route, BrowserRouter } from 'react-router-dom'
import Login from './Pages/Login'
import Home from './Pages/Home'
import Register from './Pages/Register'

const Routes = () => {
    return (
        <BrowserRouter>
            <Route path="/" component={Login}  exact />
            <Route path="/home" component={Home} />
            <Route path="/register" component={Register} />
        </BrowserRouter>
    )
}

export default Routes