<template>
  <div class="product-editor">
    <h4 class="editor-header text-center">
      {{ editMode ? "Edytuj produkt" : "Utwórz nowy produkt" }}
    </h4>
    <div v-if="errorMessages.length" class="error-messages">
      <ul>
        <li v-for="(error, index) in errorMessages" :key="index">
          {{ error }}
        </li>
      </ul>
    </div>
    <div class="form-container">
      <div class="form-group" v-if="editMode">
        <label>ID (Tylko do odczytu)</label>
        <input
          class="form-control readonly-input"
          disabled
          v-model="product.id"
        />
      </div>
      <div class="form-group">
        <label>Nazwa</label>
        <input class="form-control" v-model.trim="product.name" />
      </div>
      <div class="form-group">
        <label>Wstęp</label>
        <input class="form-control" v-model.trim="product.lead" />
      </div>
      <div class="form-group">
        <label>Opis</label>
        <textarea
          class="form-control"
          v-model.trim="product.description"
        ></textarea>
      </div>
      <div class="form-group">
        <label>Kategoria</label>
        <select class="form-control" v-model="product.categoryId">
          <option
            v-for="category in categories"
            :key="category.id"
            :value="category.id"
          >
            {{ category.name }}
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>Cena (zł)</label>
        <input
          type="number"
          class="form-control"
          v-model.number="product.price"
        />
      </div>
      <div class="form-group">
        <label>Dane techniczne (w formacie JSON)</label>
        <textarea
          class="form-control json-input"
          v-model.trim="product.technicalDetails"
          placeholder="Wprowadź dane techniczne w formacie JSON"
        ></textarea>
      </div>
      <div class="form-group">
        <label>Dodaj główne zdjęcie</label>
        <input type="file" @change="onFileSelected" class="form-control-file" />
      </div>
      <div class="form-group" v-if="product.mainImage?.url">
        <label>Obecne zdjęcie</label>
        <img
          :src="product.mainImage.url"
          alt="Główne zdjęcie"
          class="main-image-preview"
        />
      </div>
      <!-- Additional Images Section -->
      <div class="form-group" v-if="editMode">
        <label>Dodaj zdjęcia dodatkowe</label>
        <div class="adding-image">
          <button class="btn add-images-btn" @click="onAddImagesClick">
            Dodaj zdjęcia
          </button>
        </div>
        <input
          type="file"
          multiple
          ref="additionalImagesInput"
          style="display: none"
          @change="onAdditionalFilesSelected"
        />
      </div>
      <div class="form-group" v-if="editMode && product.images.length">
        <label>Obecne zdjęcia dodatkowe</label>
        <div class="additional-images">
          <div
            class="additional-image"
            v-for="image in product.images"
            :key="image.id"
          >
            <img
              :src="image.url"
              alt="Zdjęcie dodatkowe"
              class="additional-image-preview"
            />
            <button
              class="remove-image-btn"
              @click="confirmImageRemoval(image.id)"
            >
              ✖
            </button>
          </div>
        </div>
      </div>
    </div>
    <div class="button-group">
      <router-link to="/admin/products" class="btn back-btn"
        >Anuluj</router-link
      >
      <button class="btn save-btn" @click="handleSave">
        {{ editMode ? "Zapisz zmiany" : "Utwórz produkt" }}
      </button>
    </div>
    <AppModal
      v-if="successModalVisible"
      :visible="successModalVisible"
      :message="successModalMessage"
      @close="successModalVisible = false"
    />
    <ConfirmationModal
      v-if="confirmationModalVisible"
      :visible="confirmationModalVisible"
      message="Czy na pewno chcesz usunąć to zdjęcie?"
      @confirm="removeConfirmedImage"
      @close="confirmationModalVisible = false"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { storageAPI } from "@/plugins/adminAxios";
import AppModal from "@/components/modals/AppModal.vue";
import ConfirmationModal from "@/components/modals/ConfirmationModal.vue";

@Component({
  components: { AppModal, ConfirmationModal },
})
export default class ProductEditor extends Vue {
  product = {
    id: null,
    name: "",
    lead: "",
    description: "",
    categoryId: null,
    price: 0,
    technicalDetails: "",
    mainImage: null,
    images: [],
    isSaleOn: false,
    salePrice: null,
    dateSaleFrom: "",
    dateSaleTo: "",
  };

  selectedFile: File | null = null;
  selectedAdditionalFiles: File[] = [];
  errorMessages: string[] = [];
  successModalVisible = false;
  successModalMessage = "";
  confirmationModalVisible = false;
  imageToRemoveId: number | null = null;

  get editMode(): boolean {
    return this.$route.name === "ProductEdit";
  }

  get categories(): Array<any> {
    return this.$store.getters["admin/adminCategories/fullCategoryList"] || [];
  }

  async created() {
    if (!this.categories.length) {
      try {
        await this.$store.dispatch("admin/adminCategories/fetchAllCategories");
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
      }
    }

    if (this.editMode) {
      const productId = this.$route.params.id;
      try {
        await this.$store.dispatch(
          "admin/adminProducts/fetchProduct",
          productId
        );
        const product =
          this.$store.getters["admin/adminProducts/selectedProduct"];
        if (product) {
          this.product = {
            ...product,
            technicalDetails: product.technicalData
              ? JSON.stringify(JSON.parse(product.technicalData), null, 2)
              : "",
            images: product.images.map((img: any) => ({
              ...img,
              url: null,
            })),
          };

          if (this.product.mainImage?.name) {
            const response = await storageAPI.getFile(
              this.product.mainImage.name
            );
            this.product.mainImage.url = response.request.responseURL;
          }

          for (const image of this.product.images) {
            const response = await storageAPI.getFile(image.name);
            image.url = response.request.responseURL;
          }
        }
      } catch (error) {
        console.error("Błąd podczas pobierania produktu:", error);
        this.errorMessages.push("Nie udało się załadować danych produktu.");
      }
    }
  }

  async onAdditionalFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedAdditionalFiles = Array.from(input.files);
      try {
        for (const file of this.selectedAdditionalFiles) {
          const fileResponse = await this.$store.dispatch(
            "admin/adminStorage/uploadFile",
            file
          );
          if (fileResponse && fileResponse.id) {
            await this.$store.dispatch(
              "admin/adminProducts/addImageToProduct",
              {
                productId: this.product.id,
                imageId: fileResponse.id,
              }
            );
          }
        }
        this.showSuccessModal("Zdjęcia zostały pomyślnie dodane.");
        await this.$store.dispatch(
          "admin/adminProducts/fetchProduct",
          this.product.id
        );
      } catch (error) {
        this.errorMessages.push("Nie udało się dodać zdjęć.");
      }
    }
  }

  confirmImageRemoval(imageId: number) {
    this.imageToRemoveId = imageId;
    this.confirmationModalVisible = true;
  }

  async removeConfirmedImage() {
    if (this.imageToRemoveId !== null) {
      try {
        await this.$store.dispatch(
          "admin/adminProducts/removeImageFromProduct",
          {
            productId: this.product.id,
            imageId: this.imageToRemoveId,
          }
        );
        this.showSuccessModal("Zdjęcie zostało pomyślnie usunięte.");
        await this.$store.dispatch(
          "admin/adminProducts/fetchProduct",
          this.product.id
        );
      } catch (error) {
        this.errorMessages.push("Nie udało się usunąć zdjęcia.");
      } finally {
        this.confirmationModalVisible = false;
        this.imageToRemoveId = null;
      }
    }
  }

  onAddImagesClick() {
    (this.$refs.additionalImagesInput as HTMLInputElement).click();
  }

  showSuccessModal(message: string) {
    this.successModalMessage = message;
    this.successModalVisible = true;

    setTimeout(() => {
      location.reload();
    }, 1000);
  }

  validateForm(): boolean {
    this.errorMessages = [];

    if (!this.product.name) {
      this.errorMessages.push("Pole 'Nazwa' jest wymagane.");
    }
    if (this.product.price == null || this.product.price <= 0) {
      this.errorMessages.push("Cena jest wymagana i musi być większa niż 0.");
    }
    if (this.product.categoryId == null) {
      this.errorMessages.push("Pole 'Kategoria' jest wymagane.");
    }
    if (this.product.technicalDetails) {
      try {
        JSON.parse(this.product.technicalDetails);
      } catch (e) {
        console.error("Błąd walidacji JSON:", e);
        this.errorMessages.push(
          "Pole 'Dane techniczne' musi być w formacie JSON."
        );
      }
    }

    return this.errorMessages.length === 0;
  }

  async handleSave() {
    if (!this.validateForm()) return;

    const action = this.editMode ? "updateProduct" : "createProduct";
    const payload: any = {
      id: this.editMode ? this.product.id : undefined,
      name: this.product.name,
      lead: this.product.lead,
      description: this.product.description,
      price: this.product.price,
      categoryId: this.product.categoryId,
      technicalData: this.product.technicalDetails || null,
      mainImageId: this.product.mainImage?.id || null,
      isSaleOn: this.product.isSaleOn,
      salePrice: this.product.salePrice,
      dateSaleFrom: this.product.dateSaleFrom,
      dateSaleTo: this.product.dateSaleTo,
    };

    try {
      if (this.selectedFile) {
        const fileResponse = await this.$store.dispatch(
          "admin/adminStorage/uploadFile",
          this.selectedFile
        );

        if (fileResponse && fileResponse.id) {
          payload.mainImageId = fileResponse.id;
        }
      }

      await this.$store.dispatch(`admin/adminProducts/${action}`, payload);

      this.$router.push({
        name: "ProductAdmin",
        query: {
          modalMessage: this.editMode
            ? "Produkt zaktualizowany"
            : "Produkt utworzony",
        },
      });
    } catch (error) {
      console.error("Błąd podczas zapisywania produktu:", error);
      this.errorMessages.push("Wystąpił błąd podczas zapisywania produktu.");
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedFile = input.files[0];

      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        if (this.product.mainImage) {
          this.product.mainImage.url = e.target?.result as string;
        } else {
          this.product.mainImage = { url: e.target?.result as string };
        }
      };
      fileReader.readAsDataURL(this.selectedFile);
    }
  }
}
</script>

<style scoped lang="scss">
.product-editor {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f2f2f2;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  font-family: Arial, sans-serif;

  .editor-header {
    font-size: 1.8rem;
    font-weight: bold;
    color: #fff;
    background-color: #c70a0a;
    padding: 15px;
    text-align: center;
    border-radius: 8px 8px 0 0;
  }

  .form-container {
    display: flex;
    flex-direction: column;
    gap: 15px;
    margin-top: 20px;
  }

  .form-group {
    display: flex;
    flex-direction: column;

    label {
      font-size: 1rem;
      font-weight: bold;
      margin-bottom: 5px;
    }

    .form-control {
      padding: 10px;
      font-size: 1rem;
      border: 1px solid #ccc;
      border-radius: 4px;

      &.json-input {
        font-family: monospace;
        height: 150px;
        resize: vertical;
      }
    }

    .readonly-input {
      background-color: #e9ecef;
      border-color: #ced4da;
      cursor: not-allowed;
    }

    .main-image-preview {
      max-width: 100%;
      max-height: 400px;
      object-fit: contain;
      border: 1px solid #ccc;
      border-radius: 4px;
      margin-top: 10px;
      display: block;
      margin-left: auto;
      margin-right: auto;
    }

    .adding-image {
      margin: 20px;
      display: flex;
      justify-content: center;
    }

    .additional-images {
      display: flex;
      flex-wrap: wrap;
      gap: 15px;

      .additional-image {
        box-sizing: border-box;
        position: relative;
        width: 150px;
        height: 150px;
        border: 1px solid #ccc;
        border-radius: 4px;
        overflow: hidden;

        .additional-image-preview {
          width: 100%;
          height: 100%;
          object-fit: cover;
        }

        .remove-image-btn {
          box-sizing: border-box;
          position: absolute;
          top: 5px;
          right: 5px;
          background-color: #c70a0a;
          color: white;
          border: none;
          padding: 5px;
          font-size: 0.8rem;
          border-radius: 50%;
          cursor: pointer;
          width: 24px;
          height: 24px;
          display: flex;
          align-items: center;
          justify-content: center;

          &:hover {
            background-color: #a60a0a;
          }
        }
      }
    }

    .add-images-btn {
      box-sizing: border-box;
      background-color: #c70a0a;
      color: white;
      border: none;
      width: 70%;
      padding: 10px 20px;
      font-size: 1rem;
      font-weight: bold;
      border-radius: 4px;
      cursor: pointer;
      transition: background-color 0.3s ease;

      &:hover {
        background-color: #a60a0a;
      }
    }
  }

  .button-group {
    display: flex;
    justify-content: flex-end;
    gap: 15px;
    margin-top: 20px;

    .btn {
      padding: 10px 20px;
      font-size: 1rem;
      font-weight: bold;
      border-radius: 4px;
      cursor: pointer;
      text-align: center;
    }

    .back-btn {
      background-color: #666;
      color: white;
      border: none;

      &:hover {
        background-color: #555;
      }
    }

    .save-btn {
      background-color: #c70a0a;
      color: white;
      border: none;

      &:hover {
        background-color: #a60a0a;
      }
    }
  }

  .error-messages {
    background-color: #ffefef;
    border: 1px solid #ff4d4d;
    color: #c70a0a;
    padding: 10px;
    border-radius: 4px;
    margin-bottom: 15px;

    ul {
      list-style: none;
      padding: 0;
      margin: 0;

      li {
        font-size: 0.9rem;
      }
    }
  }
}

@media (max-width: 1200px) {
  .product-editor {
    max-width: 700px;

    .editor-header {
      font-size: 1.6rem;
    }

    .form-control {
      font-size: 0.9rem;
    }
  }
}

@media (max-width: 768px) {
  .product-editor {
    padding: 15px;

    .editor-header {
      font-size: 1.4rem;
      padding: 10px;
    }

    .form-container {
      gap: 10px;
    }

    .form-control {
      font-size: 0.85rem;
      padding: 8px;
    }

    .button-group {
      flex-direction: column;
      align-items: stretch;

      .btn {
        font-size: 0.9rem;
        padding: 8px;
      }
    }

    .error-messages {
      font-size: 0.85rem;

      ul li {
        font-size: 0.8rem;
      }
    }
  }
}

@media (max-width: 480px) {
  .product-editor {
    padding: 10px;

    .editor-header {
      font-size: 1.2rem;
    }

    .form-control {
      font-size: 0.8rem;
      padding: 6px;
    }

    .button-group {
      gap: 10px;

      .btn {
        font-size: 0.8rem;
        padding: 6px;
      }
    }

    .error-messages {
      font-size: 0.8rem;

      ul li {
        font-size: 0.75rem;
      }
    }
  }
}
</style>
