import React, { useState } from 'react';
import { Link, useHistory } from 'react-router-dom';
import api from '../../api';
import { Form, FormGroup, FormControl, ControlLabel, Button } from 'rsuite';
import './styles.css';

export default function Login() {
    const [email, setEmail] = useState();
    const [senha, setSenha] = useState();
    const history = useHistory();
    
    async function handleSubmit() {
        const data = {
            email,
            senha
        }

        try {
            await api.post('login', data)
            .then(async (response) => {
                sessionStorage.setItem('token', response.data.token);
                sessionStorage.setItem('id', response.data.usuario.id);
                history.push("/home");
            });
        } catch (error) {
            alert(`${error}`);
        }
    }

    return (
        <div id="login-container">
           <div id="painel-login">
               <Form id="form-login" onSubmit={handleSubmit}>
                    <h1>TariFace</h1>
                    <FormGroup className="grupo-form-login">
                        <ControlLabel>E-mail:</ControlLabel>
                        <FormControl name="email" type="email" onChange={value => setEmail(value)}/>
                    </FormGroup>

                    <FormGroup className="grupo-form-login">
                        <ControlLabel>Senha:</ControlLabel>
                        <FormControl name="password" type="password" onChange={value => setSenha(value)}/>
                    </FormGroup>
                    
                    <FormGroup className="group-form-butoes">
                            <Button type="submit" className="bnt-confirm-login" >Login</Button>
                            <Link to="/register" className="bnt-register-login" >Cadastrar</Link>
                    </FormGroup>
               </Form>
           </div>
        </div>
    )
}