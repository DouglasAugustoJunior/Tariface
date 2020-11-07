import React, { Component } from "react";
import filesize from "filesize";
import api from "../../api";
import {Alert} from "rsuite"
import { Container, Content } from "./styles";
import Upload from "./Upload";
import FileList from "./FileList";

class MultiDropzone extends Component {
  state = {
    uploadedFiles: [],
    disabledDropzone: false
  };

  async componentDidMount() {
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;
  }

  handleUpload = async files => {
    const uploadedFiles = files.map(file => ({
      file,
      id: file.name + "Id-" + Math.floor(Math.random() * 1000),
      name: file.name,
      readableSize: filesize(file.size),
      preview: URL.createObjectURL(file),
      progress: 0,
      uploaded: false,
      error: false,
      url: null
    }));
    
    this.setState({ uploadedFiles: this.state.uploadedFiles.concat(uploadedFiles) });

    uploadedFiles.forEach(this.processUpload)
  };

  updateFile = (id, data) => {
    this.setState({
      uploadedFiles: this.state.uploadedFiles.map(uploadedFile => {
        return id === uploadedFile.id
          ? { ...uploadedFile, ...data }
          : uploadedFile;
      })
    });
  };

  processUpload = async uploadedFile => {
    const id = sessionStorage.getItem('id');
    const data = new FormData();

    data.append("arquivo", uploadedFile.file, uploadedFile.name);
    this.setState({disabledDropzone: true})
    await api.post(`imagem/upload?idUsuario=${id}`, data, {
      onUploadProgress: e => {
        const progress = parseInt(Math.round((e.loaded * 100) / e.total));

        this.updateFile(uploadedFile.id, { progress });
      }
      
    }).then(response => {
      if(response.data.indexOf("erro") !== -1) throw new Error(response.data);

      if(response.data.indexOf("falha") !== -1) throw new Error(response.data);

      if(response.data.indexOf("não suportado") !== -1) throw new Error(response.data);

      if(response.data.indexOf("Nenhum") !== -1) throw new Error(response.data);

      api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then(response => {
          response.data.imagens.length >= 8 && this.props.activeButton();
          response.data.imagens.length >= 15 && window.location.reload(true);
        });

      this.setState({disabledDropzone: false});
      this.updateFile(uploadedFile.id, {
        uploaded: true,
        id: response.data._id,
        url: response.data.url
      });
    }).catch((error) => {
      Alert.error(error.toString());
      this.setState({disabledDropzone: false});
      this.updateFile(uploadedFile.id, {
        error: true
      });
    });
  };

  componentWillUnmount() {
    this.state.uploadedFiles.forEach(file => URL.revokeObjectURL(file.preview));
  }

  render() {
    const { uploadedFiles } = this.state;

    return (
      <Container>
        <Content>
          <Upload onUpload={this.handleUpload} disabledDropzone={this.state.disabledDropzone}/>
          {!!uploadedFiles.length && (
            <FileList files={uploadedFiles} />
          )}
        </Content>
      </Container>
    )
  }
}

export default MultiDropzone;
