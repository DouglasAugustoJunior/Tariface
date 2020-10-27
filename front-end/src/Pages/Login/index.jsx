import React from 'react';
import {Link} from 'react-router-dom';
import { Form, FormGroup, FormControl, ControlLabel, } from 'rsuite';
import 'rsuite/dist/styles/rsuite-default.css';
import './styles.css';

export default function Login() {
   
    return (
        <div className="login-container">
           <div className="painel-login">
               <Form id="form-login">
               <ControlLabel>TariFace</ControlLabel>
                <FormGroup>
                    <ControlLabel>E-mail</ControlLabel>
                    <FormControl name="email" type="email" />
                </FormGroup>
                <FormGroup>
                    <ControlLabel>Senha</ControlLabel>
                    <FormControl name="password" type="password" />
                </FormGroup>
                <FormGroup>
                    <div className="button-group">
                        <Link to="/home" className="bnt-login" >Login</Link>
                        <Link to="/register" className="bnt-register" >Cadastrar</Link>
                    </div>
                </FormGroup>
               </Form>
           </div>
        </div>
    )
}