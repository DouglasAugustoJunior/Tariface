import React, { Component } from "react";
import { uniqueId } from "lodash";
import filesize from "filesize";
import api from "../../api";
import { Container, Content } from "./styles";
import Upload from "./Upload";
import FileList from "./FileList";

class MultiDropzone extends Component {
  state = {
    uploadedFiles: [],
    photo: 0
  };

  // componentDidUpdate(prevProps) {
  //   if(prevProps.observer !== this.state.observer) {
  //     const http = this.state.observer
  //     // http.then((response) => {console.log("Deu bom: ", response)})
  //     console.log(typeof http)
  //   } 
  // }

  async componentDidMount() {
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;
    // const response = await api.get("posts");

    // this.setState({
    //   uploadedFiles: response.data.map(file => ({
    //     id: file._id,
    //     name: file.name,
    //     readableSize: filesize(file.size),
    //     preview: file.url,
    //     uploaded: true,
    //     url: file.url
    //   }))
    // });
  }

  handleUpload = async files => {
    const uploadedFiles = files.map(file => ({
      file,
      id: uniqueId(),
      name: file.name,
      readableSize: filesize(file.size),
      preview: URL.createObjectURL(file),
      progress: 0,
      uploaded: false,
      error: false,
      url: null
    }));
    
    this.setState({
      uploadedFiles: this.state.uploadedFiles.concat(uploadedFiles)
    });

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
    
    return await api.post(`imagem/upload?idUsuario=${id}`, data, {
      onUploadProgress: e => {
        const progress = parseInt(Math.round((e.loaded * 100) / e.total));

        this.updateFile(uploadedFile.id, {
          progress
        });
      }
    }).then(response => {
      this.setState({photo: this.state.photo + 1})
      this.updateFile(uploadedFile.id, {
        uploaded: true,
        id: response.data._id,
        url: response.data.url
      });
      this.state.photo > 8 && window.location.reload(true);
      console.log("Fotos Local: ", this.state.photo);
    }).catch((error) => {
      console.log("Fotos Local: ", this.state.photo);
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
          <Upload onUpload={this.handleUpload} />
          {!!uploadedFiles.length && (
            <FileList files={uploadedFiles} />
          )}
        </Content>
      </Container>
    );
  }
}

export default MultiDropzone;
