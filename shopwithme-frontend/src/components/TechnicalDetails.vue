<template>
  <div class="technical-details">
    <div class="image-placeholder">
      <img src="https://placehold.co/400x400" alt="Technical Image" />
    </div>
    <div class="details-content">
      <h2>Dane techniczne</h2>
      <ul>
        <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
        <li>
          Nunc vulputate libero et velit interdum, ac aliquet odio mattis.
        </li>
        <li>
          Class aptent taciti sociosqu ad litora torquent per conubia nostra.
        </li>
        <li>Nulla facilisi. Donec vulputate interdum sollicitudin.</li>
        <li>Mauris a blandit leo, id congue libero.</li>
        <li>
          Etiam dapibus eros ac metus ullamcorper, in tincidunt nulla blandit.
        </li>
        <li>
          Suspendisse potenti. Pellentesque non risus nec libero venenatis.
        </li>
        <li>Praesent elementum orci nec ligula lacinia sollicitudin.</li>
      </ul>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class TechnicalDetails extends Vue {}
</script>

<style scoped lang="scss">
.technical-details {
  display: flex;
  gap: 2rem;
  padding: 2rem 0;
  align-items: flex-start;

  .image-placeholder {
    flex: 1;
    max-width: 400px;

    img {
      width: 100%;
      border-radius: 8px;
    }
  }

  .details-content {
    flex: 2;

    h2 {
      font-size: 1.8rem;
      margin-bottom: 1rem;
      color: #333;
    }

    ul {
      list-style-type: disc;
      padding-left: 1.5rem;
      color: #555;
      font-size: 1rem;
      line-height: 1.6;
    }
  }
}
</style>
