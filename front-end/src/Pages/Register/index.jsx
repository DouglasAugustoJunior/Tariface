import React from 'react';
import {Link} from 'react-router-dom';
import { Form, FormGroup, FormControl, ControlLabel, SelectPicker } from 'rsuite';
import 'rsuite/dist/styles/rsuite-default.css';
import './styles.css';

export default function Register() {
   
    return (
        <div id="register-container">
            <Form id="form-register">
                <FormGroup>
                    <FormGroup>
                        <ControlLabel>Nome Completo</ControlLabel>
                        <FormControl name="fullName" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>CPF</ControlLabel>
                        <FormControl name="cpf" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>E-mail</ControlLabel>
                        <FormControl name="email" type="email" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Senha</ControlLabel>
                        <FormControl name="password" type="password" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Confirmar Senha</ControlLabel>
                        <FormControl name="okPassword" type="password" />
                    </FormGroup>
                </FormGroup>

                <FormGroup>
                    <FormGroup>
                        <ControlLabel>Logradouro</ControlLabel>
                        <FormControl name="adrres" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Numero</ControlLabel>
                        <FormControl name="number" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Bairro</ControlLabel>
                        <FormControl name="bairro" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>CEP</ControlLabel>
                        <FormControl name="cep" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Cidade</ControlLabel>
                        <FormControl name="city" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Estado</ControlLabel>
                        <SelectPicker />
                    </FormGroup>
                </FormGroup>

                <FormGroup>
                    <FormGroup>
                        <ControlLabel>Titular do Cartão</ControlLabel>
                        <FormControl name="donoCartao" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Numero do Cartão</ControlLabel>
                        <FormControl name="numberCard" type="text" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>Validade</ControlLabel>
                        <FormControl name="Validade" type="date" />
                    </FormGroup>

                    <FormGroup>
                        <ControlLabel>CSV</ControlLabel>
                        <FormControl name="csv" type="date" />
                    </FormGroup>
                </FormGroup>

                <FormGroup>
                    <div className="button-group">
                        <Link to="/home" className="bnt-register" >Confirmar</Link>
                        <Link to="/register" className="bnt-register" >Cancelar</Link>
                    </div>
                </FormGroup>
            </Form>
        </div>
    )
}