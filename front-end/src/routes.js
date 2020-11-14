import React from 'react'
import { Route, BrowserRouter, Switch, Redirect } from 'react-router-dom'
import Login from './Pages/Login'
import Home from './Pages/Home'
import Register from './Pages/Register'
import { isAuthenticated } from "./services/autenticacao"

const PrivateRoute = ({ component: Component, ...rest }) => (
    <Route
      {...rest}
      render={props =>
        isAuthenticated() ?
        ( <Component {...props} /> ) :
        ( <Redirect to={{ pathname: "/", state: { from: props.location } }} /> )
      }
    />
)

const PublicRoute = ({ component: Component, ...rest }) => (
    <Route
      {...rest}
      render={props =>
        !isAuthenticated() ?
        ( <Component {...props} /> ) :
        ( <Redirect to={{ pathname: "/home", state: { from: props.location } }} /> )
      }
    />
)

const Routes = () => {
    return (
        <BrowserRouter>
        <Switch>
            <PublicRoute path="/" component={Login}  exact />
            <PrivateRoute path="/home" component={Home} />
            <PublicRoute path="/register" component={Register} />
            <Route path="*" component={() => <h1>Página não encontrada.</h1>} />
        </Switch>
        </BrowserRouter>
    )
}

export default Routes