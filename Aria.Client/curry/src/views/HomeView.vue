<template>
  <div>
    <easel-canvas width="500" height="500">
      <easel-shape form="star"
                   :dimensions="[500,100]"
                   :x="0"
                   :y="0"
                   fill="#CCCCFF">
      </easel-shape>
      <easel-text text="Hi there!"
                  :x="250"
                  :y="75"
                  font="70px Calibri"
                  :color="textColor"
                  :shadow="['#000088',3,2,3]"
                  :align="['center', 'middle']">
      </easel-text>
    </easel-canvas>
  </div>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator'
  import HelloWorld from '@/components/HelloWorld.vue' // @ is an alias to /src

  @Component({
    components: {
      HelloWorld
    }
  })
  export default class HomeView extends Vue {
    textColor :string = "#009944"
  }
</script>
