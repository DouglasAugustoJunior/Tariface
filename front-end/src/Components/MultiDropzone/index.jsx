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
    photos: 0
  };

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

  handleUpload = files => {
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

    uploadedFiles.forEach(this.processUpload);
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

  processUpload = uploadedFile => {
    const id = sessionStorage.getItem('id');
    const data = new FormData();

    data.append("arquivo", uploadedFile.file, uploadedFile.name);
    
    // if(this.state.photo <= 8) {
      api.post(`imagem/upload?idUsuario=${id}`, data, {
        onUploadProgress: e => {
          const progress = parseInt(Math.round((e.loaded * 100) / e.total));

          this.updateFile(uploadedFile.id, {
            progress
          });
        }
      }).then(response => {
        this.setState({photo: this.state.photo + 1});
        this.updateFile(uploadedFile.id, {
          uploaded: true,
          id: response.data._id,
          url: response.data.url
        });
      }).catch(() => {
        this.updateFile(uploadedFile.id, {
          error: true
        });
      });
    // }
  };

  handleDelete = async id => {
    // await api.delete(`posts/${id}`);

    this.setState({
      uploadedFiles: this.state.uploadedFiles.filter(file => file.id !== id)
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
            <FileList files={uploadedFiles} onDelete={this.handleDelete} />
          )}
        </Content>
      </Container>
    );
  }
}

export default MultiDropzone;
